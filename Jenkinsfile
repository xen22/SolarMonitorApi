#!/usr/bin/groovy

// Build constants
def VERSION_STRING_DEFAULT = "0.3.0" // Note: change version string here after branching a stable branch
def PUSH_TO_PRODUCTION_DEFAULT = false
def PAUSE_AFTER_BUILD_DEFAULT = false
def BUILD_CONFIG_DEFAULT = "Debug"

properties([[$class: 'ParametersDefinitionProperty',
  parameterDefinitions: [
    [$class: 'BooleanParameterDefinition',
      defaultValue: PUSH_TO_PRODUCTION_DEFAULT,
      description: 'This will publish the code on Azure (solarmonitornz.azurewebsites.net/api).\nNote: This step can only be performed on a stable branch. \nChanges will go live when selecting this!', 
      name: 'PUSH_TO_PRODUCTION'],
    [$class: 'StringParameterDefinition',
      defaultValue: VERSION_STRING_DEFAULT,
            description: 'String used to version all binaries on the master branch (along with the build number).', 
      name: 'VERSION_STRING'],
    [$class: 'ChoiceParameterDefinition',
      choices: 'Debug', // ['Debug', 'Release'], 
      description: 'Specifies which configuration to build.',
      name: 'BUILD_CONFIG'],
    [$class: 'BooleanParameterDefinition',
      defaultValue: PAUSE_AFTER_BUILD_DEFAULT,
      description: 'Pause at the end of the Build stage so that the build container used by the build is still available for inspection.',
      name: 'PAUSE_AFTER_BUILD']]]])
     

/*
// Note: this may be a new way of defining parameters but Jenkins' multibranch pipeline plugin does not accept them

properties [
    pipelineTriggers([]), 
    parameters([
        booleanParam(
            defaultValue: false, 
            description: 'This will publish the code on Azure (solarmonitornz.azurewebsites.net/api).\nNote: This step can only be performed on a stable branch. \nChanges will go live when selecting this!', 
            name: 'PUSH_TO_PRODUCTION'), 
        stringParam(
            defaultValue: '0.1.0', 
            description: 'String used to version all binaries on the master branch (along with the build number).', 
            name: 'VERSION_STRING'), 
        choiceParam(
            choices: ['Debug', 'Release'], 
            description: 'Specifies which configuration to build.', 
            name: 'BUILD_CONFIG'), 
        booleanParam(
            defaultValue: false, 
            description: 'Pause at the end of the Build stage so that the build container used by the build is still available for inspection.', 
            name: 'PAUSE_AFTER_BUILD')])]
*/

def newVersion = "${VERSION_STRING}.${env.BUILD_NUMBER}"
def rootDirectory = ""
def buildScriptsDir = "build"
def outputDirectory = "_output"

//def dockerImagesDir = "/var/lib/jenkins/jenkins-docker/SolarMonitor"
def releaseBranchPrefix = "rel-"
def productionGitTag = "GA_RELEASE" // General Availability
def netcoreappVersion = ""
def dockerImagePrefix = "jenkins"
def dockerWebApiImagePrefix = "solarmonitor"
//def checkErrorResultAndLog = "res=\$? ; if [[ \$res -ne 0 ]] ; then echo \"Error: script failed! Status returned: \$res\" ;  fi"
//def checkErrorResultAndLog = 'res=$? ; if [[ $res -ne 0 ]] ; then echo "Error: script failed! Status returned: $res" ;  fi'

def testSystemIpAddress = "10.0.0.99"
def testSystemSshUser = "sol"

def stagingSystemIpAddress = "10.0.0.99"
def stagingSystemSshUser = "sol"

def productionSystemIpAddress = "pi-solar"
def productionSystemSshUser = "sol"


if (env.BRANCH_NAME == "master") {
    newVersion = "${VERSION_STRING}.${env.BUILD_NUMBER}"
} else if (env.BRANCH_NAME.startsWith(releaseBranchPrefix)) {
    newVersion = env.BRANCH_NAME.replace(releaseBranchPrefix, "") + "." + env.BUILD_NUMBER
} else {
    // other development branches
    newVersion = "0.0.0.${env.BUILD_NUMBER}"
}

def gitScm = null

// ----------------------------------------------------------------------------
// Test variables
// ----------------------------------------------------------------------------
echo "New version: ${newVersion}"
def minorVersion = "0";
if(newVersion.split('\\.').size() > 1) {
    minorVersion = newVersion.split('\\.')[1];
}
// unlikely that we reached such a high number - but just in case 
if (minorVersion.toInteger() >= 255) {
    minorVersion = (minorVersion.toInteger() % 255).toString()
} 
def testSubnet = "172.18.${minorVersion}.0"
def testSubnetName = "testnet${minorVersion}"
// ----------------------------------------------------------------------------


def printBuildInfo() {
    echo "\n\n--------------------------------------------------------------------------------------------------------------"
    echo "   Build constants"
    echo "--------------------------------------------------------------------------------------------------------------"
    echo "BRANCH_NAME: ${env.BRANCH_NAME}"
    echo "BUILD_NUMBER: ${env.BUILD_NUMBER}"
    // parameters
    echo "PUSH_TO_PRODUCTION: ${PUSH_TO_PRODUCTION}";
    echo "VERSION_STRING: ${VERSION_STRING}";
    echo "BUILD_CONFIG: ${BUILD_CONFIG}";
    echo "PAUSE_AFTER_BUILD: ${PAUSE_AFTER_BUILD}";
    echo "--------------------------------------------------------------------------------------------------------------\n\n"
}

try {

    printBuildInfo() 

    echo "\n\n========================   STAGE: INIT   =================================================================\n"
    
    // Purpose: sets up global variables and Jenkins UI - this stage only performs steps
    //          on the master node and is not slave specific
    // 
    // Steps performed in INIT:
    // - setup up build variables (e.g. build version)
    // - update current build's displayName in the UI
    
    stage('Init')
    
        node('master') {
            wrap([$class: 'TimestamperBuildWrapper']) {
                def newBuildName = newVersion
                try {
                    if (newBuildName) {
                        manager.build.displayName = newBuildName
                    }
                    println "Build display name is set to ${newBuildName}"
                } catch (MissingPropertyException e) {
                    echo "Unable to change build display name."
                }
                
                // Note: this repository has been removed and the docker containers are now stored in the SolarMonitor repo
                // (this was done because docker containers configurations need to be branched at the same time as the SolarMonitor repo)
                // clone/update jenkins-docker project
                //sh "if [ ! -d ${dockerImagesDir} ]; then cd \$HOME ; git clone gitolite3@ciprian-desktop:jenkins-docker.git ; else cd \$HOME/jenkins-docker ; git pull ; fi"
            }
        }
//    }

    echo "\n\n========================   STAGE: CHECKOUT   =============================================================\n"

    // Purpose: checks out source code and stashes it
    //          This is necessary to avoid multiple git checkouts (and to prevent Jenkins
    //          from showing the same commits multiple times in the web interface)
    // 
    // Steps performed in CHECKOUT:
    //   - clean _checkout directory (all source code from previous checkout)
    //   - check out code from git (from the correct branch)
    //   - tag git repo with the new version
    //   - stash source code (used by the build and deployment steps - staging and production)
    //   - stash docker data (used by the later stages to create their own docker images)

    stage("Checkout")
    
        def sourceCodeStashName = "source_code_stash"
        def dockerDataStashName = "docker-data"
        
        node {
            wrap([$class: 'TimestamperBuildWrapper']) {
                dir ("_checkout") {
                    deleteAllFiles()
                    
                    gitScm = git url: 'gitolite3@ciprian-desktop:SolarMonitor.git', branch: env.BRANCH_NAME
                    
                    sh "git tag -a ${newVersion} -m '' "
                    sh "git push --follow-tags origin ${newVersion}"
                    
                    // save the version in a local file so that we can print it when extracing
                    // the stash (in the later stages)
                    sh "echo ${newVersion} > ./VERSION"

                    // read .NET Core App version into a variable (used by later stages to avoid hard coding it in this script)
                    netcoreappVersion = readFile('./NETCOREAPP_VERSION').trim()
                    
                    // stash source code as it's needed by the later stages ("build", staging" and "production")
                    stash name: sourceCodeStashName, useDefaultExcludes: false // include everything since this is a clean checkout

                    // stash docker data
                    dir("docker") {
                        stash name: dockerDataStashName, useDefaultExcludes: false
                    }
                }
            }
        }
//    }
    
    echo "\n\n========================   STAGE: BUILD   ================================================================\n"

    // Purpose: runs the build and related steps. Multiple platform-specific build steps
    //          are run in parallel on different build slaves.
    // 
    // Steps performed in BUILD:
    // 1. Linux
    //   * on master node
    //      - unstash docker-data stash containing the Dockerfiles
    //      - set up the docker build image (or reuse existing one)
    //      - start a container from it
    //   * inside the docker Linux build container
    //      - clean current workspace (all source code and binaries from previous checkout and build)
    //      - unstash source code (checked out in the previous stage)
    //      - run dotnet restore to update dependencies
    //      - run build/clean_build.sh to remove all binaries (not needed anymore since we're doing clean checkout now)
    //      - update version info in project.json files
    //      - run actual build (i.e. run a script that invokes dotnet build in each source dir)
    //      - check version of each dll/exe built
    //      - create packages (currently only for the SolarMonitor.Simulator tool)
    //      - archive packages on jenkins
    //      - TODO: build documentation with docfx (docfx currently crashes on Linux, as of v. 2.1.0-cli-alpha)
    //      - stash unit test binaries
    //      - and integration test binaries
    //      - stash test scripts 
    //      - stash simulator tool (currently used by the smoke tests)
    //      - publish main SolarMonitor.Web.Api application 
    //      - stash publish directory - used by the next stage (Publish) to create a Docker image
    //
    // 1. Windows 
    //   * inside the windows build slave (runs on a separate, physical machine - laptop)
    //      - clean current workspace (all source code and binaries from previous checkout and build)
    //      - unstash source code (checked out in the previous stage)
    //      - run dotnet restore to update dependencies
    //      - run build/clean_build.sh to remove all binaries (not needed anymore since we're doing clean checkout now)
    //      - update version info in project.json files
    //      - run actual build (i.e. run a script that invokes dotnet build in each source dir)
    //      - check version of each dll/exe built
    //      - build documentation with docfx
    //      - publish documentation in Jenkins 
    //      - create packages (currently only for the SolarMonitor.Simulator tool)
    //      - archive packages on jenkins


    stage('Build')
        
        def unit_tests_stash_prefix = "unit_tests_${BUILD_CONFIG}_${newVersion}"
        def integration_tests_stash_prefix = "integration_tests_${BUILD_CONFIG}_${newVersion}"
        def tools_stash_prefix = "tools_${BUILD_CONFIG}_${newVersion}"
        
        parallel (
        'Linux build': {
            node("master") {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    deleteAllFiles()

                    dir("_docker") {
                        deleteAllFiles()
                        unstash dockerDataStashName
                    }
                    def dockerBuildImg = createDockerImage("${dockerImagePrefix}/build-slave-netcoreapp${netcoreappVersion}", 
                                                        "./_docker/docker-build-slave")

                    def contName = "docker-build-slave-${newVersion}"
                    def dockerBuildSlaveArgs = "--name ${contName} " + 
                    "-v /var/lib/jenkins/.nuget:/var/lib/jenkins/.nuget " + 
                    "-v /var/lib/jenkins/.local:/var/lib/jenkins/.local ";
                    
                    echo "About to create and switch to the ${contName} container (via Image.inside())"
                    dockerBuildImg.inside(dockerBuildSlaveArgs) {
                        checkDirEmpty("_docker*")
                        unstash sourceCodeStashName
                    
                        // this is not really necessary as it's a clean git repo
                        // leaving it in as confirmation that the dir is indeed clean of binaries
                        sh "${buildScriptsDir}/clean_build.sh ."
                    
                        sh "${buildScriptsDir}/update_version.sh ${newVersion}"
                        sh "dotnet restore"
                        
                        sh "mkdir -p ${outputDirectory}"
                        
                        sh "${buildScriptsDir}/build_all.sh ${BUILD_CONFIG}"
                        sh "${buildScriptsDir}/check_version.sh ${newVersion}"
                        
                        sh "${buildScriptsDir}/create_packages.sh ${BUILD_CONFIG} ${newVersion} Linux"
                        def archives = "${outputDirectory}/*.tar.gz"
                        step([$class: 'ArtifactArchiver', artifacts: archives, fingerprint: true])
                        
                        // echo "Generating docs."
                        // sh "docfx doc/docfx_project/docfx.json"
                        // publishHTML(target: [
                        //     reportName: "API Documentation",
                        //     reportDir: "output/docfx_site",
                        //     reportFiles: "index.html",
                        //     allowMissing: false,
                        //     alwaysLinkToLaskBuild: false,
                        //     keepAll: true])
                        
                        def bin_dir_expr = "test/unit/**/bin/${BUILD_CONFIG}/**"
                        stash name: "${unit_tests_stash_prefix}_linux", 
                            includes: "${bin_dir_expr}/*.dll, ${bin_dir_expr}/*.json, test/unit/**/project*.json"
                            
                        bin_dir_expr = "test/integration/**/bin/${BUILD_CONFIG}/**"
                        stash name: "${integration_tests_stash_prefix}_linux",
                            includes: "${bin_dir_expr}/*.dll, ${bin_dir_expr}/*.json, test/integration/**/project*.json"
                            
                        stash name: "scripts_stash"
                            include: "${buildScriptsDir}/*, NETCOREAPP_VERSION"
                            
                        bin_dir_expr = "tools/**/bin/${BUILD_CONFIG}/**"
                        stash name: "${tools_stash_prefix}_linux",
                            includes: "${bin_dir_expr}/*.dll, ${bin_dir_expr}/*.exe"
                            
                        def webApiDir = "src/SolarMonitor.Web.Api" 
                        sh "dotnet publish ${webApiDir}"
                        stash name: "solarmonitor-webapi-stash",
                            includes: "${webApiDir}/bin/${BUILD_CONFIG}/netcoreapp${netcoreappVersion}/publish/**/*, ${webApiDir}/Dockerfile, ${webApiDir}/appsettings.json"

                        sh "${buildScriptsDir}/generate_db_schema.sh"

                        def dbDir = "src/SolarMonitorDb/src/" 
                        def dbOutputDir = "${outputDirectory}/db" 
                        sh "mkdir -p ${dbOutputDir}"
                        sh "cp ${dbDir}/SampleData.sql ${dbDir}/recreate_db.sh ${dbDir}/ef/SolarMonitorDbSchema.sql ${dbOutputDir}" 
                        stash name: "solarmonitor-db-stash",
                            includes: "${dbOutputDir}/*"

                        // create tar.gz and archive db 
                        def dbArchive = "${outputDirectory}/db-${newVersion}.tar.gz"
                        sh "tar zcf ${dbArchive} ${dbOutputDir}"
                        step([$class: 'ArtifactArchiver', artifacts: dbArchive, fingerprint: true])
                        
                        if (PAUSE_AFTER_BUILD == "true") {
                            input "Debug: container will be deleted after this point."
                        }
                    }
                }
            }
        }, 'Windows build': {
            node('windows-build') {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    dir ("_build") {
                        def bash_exe = "\"c:\\Program Files (x86)\\git\\bin\\bash.exe\""
                        // Uncomment this to use the Bash that is now built into Windows 10 (via WSL) 
                        //def bash_exe = "bash"

                        echo "Using bash executable: ${bash_exe}"
                        bat "${bash_exe} --version"
                        bat "dir"
                        bat "${bash_exe} -c \"for dir in \$(ls -Ab) ; do rm -rf \$dir ; done\" "
                        bat "dir"
                        
                        unstash sourceCodeStashName
                        
                        bat "${bash_exe} -c \"./${buildScriptsDir}/clean_build.sh .\""
                        bat "dotnet restore"
                        bat "${bash_exe} -c \"./${buildScriptsDir}/update_version.sh ${newVersion}\""
                        
                        bat "${bash_exe} -c \"mkdir -p ${outputDirectory}\""
                        bat "${bash_exe} -c \"./${buildScriptsDir}/build_all.sh ${BUILD_CONFIG}\""
                        
                        bat "${bash_exe} -c \"${buildScriptsDir}/check_version.sh ${newVersion}\""

                        echo "Calling docfx help in order to get the version printed."
                        bat "docfx help"
                        echo "Generating docs."
                        bat "docfx doc/docfx_project/docfx.json"
                        publishHTML(target: [
                            reportName: "API Documentation",
                            reportDir: "${outputDirectory}/docfx_site",
                            reportFiles: "index.html",
                            allowMissing: false,
                            alwaysLinkToLaskBuild: false,
                            keepAll: true])
                        
                        bat "${bash_exe} -c \"${buildScriptsDir}/create_packages.sh ${BUILD_CONFIG} ${newVersion} Windows\""
                        def archives = "${outputDirectory}/*.tar.gz"
                        step([$class: 'ArtifactArchiver', artifacts: archives, fingerprint: true])    
                    }
                }
            }
        }
        )
//    }
    
    
    echo "\n\n========================   STAGE: PUBLISH   ==============================================================\n"
    
    // Purpose: creates a Docker image and publishes the Web API application to it
    //          The image will be stored in a local Docker registry.
    // 
    // Steps performed in PUBLISH:
    //   * on master node
    //      - unstash docker-data stash containing the Dockerfiles
    //      - create a base image if it doesn't already exist
    //      - get the stash containing Web API application built and published under the previous stage
    //      - build new docker image (based on the base image) using the Dockerfile from src/SolarMonitor.Web.Api
    //        that was published in the Build stage above - this image automatically copies all binaries from the 
    //        publish directory into the /app directory in the container where the app will run from

    stage('Publish') 

        node("master") {
            wrap([$class: 'TimestamperBuildWrapper']) {

                dir("_docker") {
                    deleteAllFiles()
                    unstash dockerDataStashName
                }

                dir("_publish") {
                    deleteAllFiles()
                    
                    // create base image if it doesn't already exist
                    def baseImage = createDockerImage("${dockerImagePrefix}/webapi-base-netcoreapp${netcoreappVersion}", "../_docker/docker-webapi-base")
                    
                    unstash "solarmonitor-webapi-stash"
                    dir("src/SolarMonitor.Web.Api") {
                        sh "ls -la"
                        sh "ls -la bin/Debug/netcoreapp${netcoreappVersion}/publish"
                        //sh "dotnet publish"
                        def dockerBuildImg = createDockerImage("${dockerWebApiImagePrefix}/webapi-${newVersion}", ".")
                    }

                    unstash "solarmonitor-db-stash"
                    
                    echo "Setting up remote dir."
                    sh "ssh ${testSystemSshUser}@${testSystemIpAddress} mkdir -p /home/sol/test/db-${newVersion}"

                    echo "Upload db files."
                    sh "scp ${outputDirectory}/db/* ${testSystemSshUser}@${testSystemIpAddress}:test/db-${newVersion}/"

                }
            }
        }
//    }
    
    echo "\n\n========================   STAGE: TEST   =================================================================\n"
    
    // Purpose: runs various tests in parallel using Docker containers
    // 
    // Steps performed in TEST:
    // 1. Unit tests
    //   * on master node
    //      - unstash docker-data stash containing the Dockerfiles
    //      - set up a docker test image if it doesn't exist
    //      - start a container from it
    //   * inside a docker Linux test container
    //      - unstash unit test binaries
    //      - run 'dotnet test' in each test directory
    //      - stash test artifacts (result xml files)
    //
    // 2. Integration tests
    //   * on master node
    //      - unstash docker-data stash containing the Dockerfiles
    //      - set up a private docker subnet - this will contain the SUT container (i.e. the web API app) and the
    //        integration test container - the subnet must be unique in case other builds are running at the same time
    //      - start the SUT container from an image created in the Publish stage
    //      - set up a docker test image if it doesn't exist
    //      - start a container from it - inside this container:
    //        - unstash integration test binaries
    //        - run 'dotnet test' in the integration test directory
    //        - archive test artifacts (result xml files)
    //
    // 3. Smoke tests
    //   * on master node
    //      - unstash simulator tool and scripts
    //      - run test
    //      - TODO: archive test results (need to modify bash script to output to a junit xml file)
    
    stage('Test')
        
        parallel ('Unit': {
            node("master") {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    deleteAllFiles()
                    dir("_docker") {
                        deleteAllFiles()
                        unstash dockerDataStashName
                    }
                    def dockerBuildImg = createDockerImage("${dockerImagePrefix}/unit-test-slave-netcoreapp${netcoreappVersion}", 
                                                        "_docker/docker-unit-test-slave")
                    def contName = "docker-unit-test-slave-${newVersion}"
                    def dockerBuildSlaveArgs = "--name ${contName} " + 
                    "-v /var/lib/jenkins/.nuget:/var/lib/jenkins/.nuget " + 
                    "-v /var/lib/jenkins/.local:/var/lib/jenkins/.local ";
                    echo "About to create and switch to the ${contName} container (via Image.inside())"
                    dockerBuildImg.inside(dockerBuildSlaveArgs) {
                        checkDirEmpty("_docker*")          
                        unstash "${unit_tests_stash_prefix}_linux"
                        sh "mkdir -p ${outputDirectory}"
                        try {
                        sh "find test/unit -mindepth 1 -maxdepth 1 -type d -exec dotnet test {} -xml {}_TestResults.xml \\;  ; mv test/unit/*xml ${outputDirectory}"
                        } catch (err) {
                        echo "Unit tests failed"
                        }
                        def testResultsPattern = "${outputDirectory}/*UnitTests_TestResults.xml"
        
                        step([$class: 'ArtifactArchiver', artifacts: testResultsPattern, fingerprint: false])
                        
                        stash name: "unit_test_results_stash"
                            include: testResultsPattern
                    }
                }
            }
        } , 'Integration': {
            node("master") {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    deleteAllFiles()
                    
                    // start the container hosting the Web api service
                    // Firstly, create a new network
                    echo "[docker] Creating network ${testSubnet}/24 ${testSubnetName}"
                    sh "docker network create --subnet=${testSubnet}/24 ${testSubnetName}"
                    def webapiImage = docker.image("solarmonitor/webapi-${newVersion}")
                    def apiContainerName = "webapi-container-${newVersion}"
                    def finalByte = ((env.BUILD_NUMBER).toInteger() % 254).toString()
                    def apiContainerIpAddr = testSubnet.substring(0, testSubnet.length() - 1) + finalByte
                    echo "[docker] About to start container ${apiContainerName} configured with IP ${apiContainerIpAddr}"
                    webapiImage.withRun("--name ${apiContainerName} --net ${testSubnetName} --ip ${apiContainerIpAddr}") {
                        dir("_docker") {
                            deleteAllFiles()
                            unstash dockerDataStashName
                        }
                        def dockerBuildImg = createDockerImage("${dockerImagePrefix}/integration-test-slave-netcoreapp${netcoreappVersion}", 
                                                            "_docker/docker-integration-test-slave")
                        def contName = "docker-integration-test-slave-${newVersion}"
                        def integrationContainerIpAddr = testSubnet.substring(0, testSubnet.length() - 1) + "254"
                        def dockerBuildSlaveArgs = "--name ${contName} " + 
                        "--net ${testSubnetName} " + 
                        "--ip ${integrationContainerIpAddr} " +
                        "-v /var/lib/jenkins/.nuget:/var/lib/jenkins/.nuget " + 
                        "-v /var/lib/jenkins/.local:/var/lib/jenkins/.local " + 
                        "-e WEB_API_CONTAINER_IPP_ADDR=${apiContainerIpAddr}";
                        echo "About to create and switch to the ${contName} container (via Image.inside())"
                        dockerBuildImg.inside(dockerBuildSlaveArgs) {
                            checkDirEmpty("_docker*")
                            
                            unstash "${integration_tests_stash_prefix}_linux"
                            sh "mkdir -p ${outputDirectory}"
                            try {
                            sh "find test/integration -mindepth 1 -maxdepth 1 -type d -exec dotnet test {} -xml {}_TestResults.xml \\;  ; mv test/integration/*xml ${outputDirectory}"
                            } catch (err) {
                            echo "Integration tests failed"
                            }
                            def testResultsPattern = "${outputDirectory}/*IntegrationTests_TestResults.xml"
                            
                            step([$class: 'ArtifactArchiver', artifacts: testResultsPattern, fingerprint: false])
            
                            stash name: "integration_test_results_stash"
                                include: testResultsPattern
                        }
                    }
                }
            }
        }, 'Smoke': {
            node("master") {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    deleteAllFiles()
                    dir("_docker") {
                        deleteAllFiles()
                        unstash dockerDataStashName
                    }
                    def dockerBuildImg = createDockerImage("${dockerImagePrefix}/smoke-test-slave-netcoreapp${netcoreappVersion}", 
                                                        "_docker/docker-smoke-test-slave")
                    def contName = "docker-smoke-test-slave-${newVersion}"
                    def dockerBuildSlaveArgs = "--name ${contName}" 
                    echo "About to create and switch to the ${contName} container (via Image.inside())"
                    dockerBuildImg.inside(dockerBuildSlaveArgs) {
                        checkDirEmpty("_docker*")          
                        unstash "scripts_stash"
                        unstash "${tools_stash_prefix}_linux"
                        sh "${buildScriptsDir}/run_smoke_tests.sh ${BUILD_CONFIG} ${newVersion}"
                    }
                }
            }
        })
//    }
    
    
    echo "\n\n========================   STAGE: STAGING   ==============================================================\n"
    
    // Purpose: deploy application to a test server on Azure
    // 
    // Steps performed in STAGING:
    //   - check branch name: we only perform this step for stable release branched (prefixed with 'rel-')
    //   - unstash source code (stashed during the Linux build stage) into a temp dir
    //   - TODO: update deployment branch in settings.xml on the server
    //   - push to the staging remote on Azure (solarmonitor-test.scm.azurewebsites.net)
    
    stage (name: 'Staging', concurrency: 1)
        
        echo "Checking branch name (${env.BRANCH_NAME}) to see if we are allowed to push to staging."
        if (!env.BRANCH_NAME.startsWith(releaseBranchPrefix)) {
            echo "Skipping Staging stage."
        } else {
            node {
                wrap([$class: 'TimestamperBuildWrapper']) {
                    withCredentials([[$class: 'UsernamePasswordBinding', 
                                    credentialsId: '93b62e0b-7062-40b4-8be3-9ee0f7ed914c', 
                                    variable: 'azureCredentials']]) {
                                    
                        dir ("_staging") {
                            deleteAllFiles()
                            unstash sourceCodeStashName
                            sh "cat ./VERSION"

                            def azureStagingRepo = "https://${env.azureCredentials}@solarmonitor-test.scm.azurewebsites.net:443/solarmonitor-test.git"
                            def site = "solarmonitor-test"
                            pushToAzureRepo(azureStagingRepo, site, env.BRANCH_NAME)
                        }
                    }
                }
            }
        }
//    }
    
    
    echo "\n\n========================   STAGE: PRODUCTION   ===========================================================\n"
    
    // Purpose: deploy application to the production server on Azure
    //          (this stage is almost identical to the staging, except that it's only performed manually)
    // 
    // Steps performed in PRODUCTION:
    //   - check branch name: we only perform this step for stable release branched (prefixed with 'rel-')
    //   - if user ticks the PUSH_TO_PRODUCTION checkbox:
    //     - wait for user to confirm deployment
    //     - unstash source code (stashed during the Linux build stage) into a temp dir
    //     - TODO: update deployment branch in settings.xml on the server
    //     - push to the production remote on Azure (solarmonitornz.scm.azurewebsites.net)
    //     - tag the branch in git with "GA_RELEASE" 
    //     - if deployment was successful, insert a "badge" next to the build to make
    //       it clear this build was deployed to production

    stage (name: 'Production', concurrency: 1)

        echo "Checking branch name (${env.BRANCH_NAME}) to see if we are allowed to push to production."
        if (!env.BRANCH_NAME.startsWith(releaseBranchPrefix)) {
            echo "Skipping Production stage."
        } else {
            if(PUSH_TO_PRODUCTION == "false") {
            echo "Production stage SKIPPED."
            } else {
                input 'Confirm deploy to production?'
                node {
                    wrap([$class: 'TimestamperBuildWrapper']) {
                        echo "Pushing to production."
                        withCredentials([[$class: 'UsernamePasswordBinding', 
                                        credentialsId: '93b62e0b-7062-40b4-8be3-9ee0f7ed914c', 
                                        variable: 'azureCredentials']]) {
                                        
                            dir ("_production") {
                                deleteAllFiles()
                                unstash sourceCodeStashName
                                sh "cat ./VERSION"

                                def azureProductionRepo = "https://${env.azureCredentials}@solarmonitornz.scm.azurewebsites.net:443/solarmonitornz.git"
                                def site = "solarmonitornz" 
                                pushToAzureRepo(azureProductionRepo, site, env.BRANCH_NAME)

                                echo "Deployment successful. Tagging source code."
                                sh "git tag ${productionGitTag}"
                                sh "git push origin ${productionGitTag}"
                                
                                echo "Deployment successful. Updating status badge."
                                manager.addBadge("green.gif", "GA Release. Deployed to production server.")
                            }
                        }
                    }
                }
            }
        }
//    }

} finally {

    echo "\n\n========================   STAGE: FINALISE   =============================================================\n"
    
    // Purpose: cleanup stage at the end of the pipeline
    //          (this stage is always executed, whether the pipeline succeeds or fails)
    // 
    // Steps performed in FINALISE:
    //   - unstash test results
    //   - parse test results and publish to Jenkins
    //   - parse console log in jenkins for errors and warnings
    //     (this uses the jenkins-rule-logparser script on master jenkins node, in /var/lib/jenkins)
    //     -> if any errors are found, the build is marked as failed, 
    //        on any warnings it's marked unstable
    //   - docker cleanup (currently: delete integration testing subnet)
    
    stage('Finalise')
        node('master') {
            wrap([$class: 'TimestamperBuildWrapper']) {
                deleteAllFiles()
                try {
                    unstash "unit_test_results_stash"
                    unstash "integration_test_results_stash"
        
                    def testResultsPattern = "${outputDirectory}/*_TestResults.xml"
                    step([$class: 'XUnitPublisher', testTimeMargin: '3000', thresholdMode: 1, 
                        thresholds: [[$class: 'FailedThreshold', failureNewThreshold: '', failureThreshold: '', unstableNewThreshold: '0', unstableThreshold: '0'], 
                            [$class: 'SkippedThreshold', failureNewThreshold: '', failureThreshold: '', unstableNewThreshold: '0', unstableThreshold: '0']], 
                        tools: [[$class: 'XUnitDotNetTestType', deleteOutputFiles: true, failIfNotNew: true, pattern: testResultsPattern, skipNoTestFiles: false, stopProcessingIfError: true]]])
                } catch(ex) {
                    echo "[Finalise] Error: failed to retrieve stashes with the test results or to publish them. Was the test stage skipped?"
                    echo "Exception caught: ${ex}"
                }
            
                step([$class: 'LogParserPublisher', 
                    parsingRulesPath: '/var/lib/jenkins/jenkins-rule-logparser', 
                    failBuildOnError: true, 
                    unstableOnWarning: true,
                    showGraphs: true,
                    useProjectRule: false])

                // Note: disabled for now since JiraIssueUpdater doesn't seem to support multi-branch pipeline builds yet
                // See: http://ciprian-desktop:8080/job/SolarMonitor/job/master/34/console
                // exception: java.lang.IllegalArgumentException: Unsupported run type org.jenkinsci.plugins.workflow.job.WorkflowRun
                // Note2: this may not be necessary if we get the Jenkins plugin in JIRA to work (which is also not supporting pipeline builds yet)

                // step([$class: 'hudson.plugins.jira.JiraIssueUpdater', 
                //     issueSelector: [$class: 'hudson.plugins.jira.selector.DefaultIssueSelector'], 
                //     scm: gitScm,
                //     labels: [ "$newVersion", "jenkins" ]])
                //

                gitScm = null

                sh "docker network ls"
                echo "Delete the test network ${testSubnetName} - ${testSubnet} (used by integration tests)."
                sh "docker network rm ${testSubnetName}"
                sh "docker network ls"
            }
        }
//    }
}    


def deleteAllFiles() {
    sh "ls -laF"
    sh 'for dir in `ls -Ab` ; do rm -rf $dir ; done'
    sh "ls -laF"
    checkDirEmpty()
}

def checkDirEmpty(def pattern = "") {
    sh 'if [ "`ls -A -I \"' + pattern + '\"`" ]; then echo "Error: unable to delete all files in dir: $PWD"; else echo "Empty dir: $PWD" ; fi'
}

def printDockerImages() {

    echo "--------------------------------------------------------------------------"
    runScript("docker images", "List docker images", false, false)
    echo "--------------------------------------------------------------------------"
    runScript("docker ps -a", "List docker containers", false, false)
    echo "--------------------------------------------------------------------------"
}

def runScript(def script, def label = "", def echoScript = true, def checkResult = true) {
    echo "[${label}]"
    if(echoScript) {
        echo "Running bash script: \"${script}\""
    }
    if(checkResult) {
        sh script + " ; res=\$? ; set +x ; if [ \$res -ne 0 ] ; then echo \"Error: script failed! Status returned: \$res\" ; fi ; exit \$res"
    } else {
        sh script
    }
}

def createDockerImage(def dockerImageName, def dockerfileLocation) {
    
    printDockerImages()
    
    def dockerBuildImg = docker.image(dockerImageName)
    runScript("docker images -q ${dockerImageName} >./docker_img.txt", "Query docker image ${dockerImageName}")
          
    id = readFile("./docker_img.txt").trim()
    if(id == "") {
        echo "No image with name ${dockerImageName} found. Generating a new docker image for the Linux build based on Dockerfile from ${dockerfileLocation}."
        try {
            dockerBuildImg = docker.build(dockerImageName, dockerfileLocation)
        } catch(Exception ex) {
            echo "Error: failed to create image \"${dockerImageName}\". Exception: " + ex.toString()
            throw ex
        }
    } else {    
        echo "Image ${dockerImageName} with ID \"${id}\" already exists. Reusing it."
    }
    sh "rm ./docker_img.txt"

    printDockerImages()

    return dockerBuildImg
}

def pushToAzureRepo(def repo, def site, def branch) {
    sh "azure --version"

    // Note: logging in is done manually via the following commands:
    // $ azure account download
    // $ azure account import ./downloads/Pay-As-You-Go-8-4-2016-credentials.publishsettings

    // Note: setting the branch returns an error, which aborts the staging process
    //sh "azure site repository branch ${branch} ${site}"
    //sh "azure site repository sync"
    // The way we do it now is by editing /site/deployment/settings.xml on https://solarmonitor-test.scm.azurewebsites.net
    // (use the Kudu portal -> Debug console or Filezilla to upload file directly)

    echo "Show current status of the site (including deployment branch). "
    sh "azure site show ${site}"
    //echo "IMPORTANT: If branch is incorrect, issue the following command manually: \"azure site repository branch ${branch} ${site}\" "
    echo "Note: deployment will be done for branch specified in /site/deployment/settings.xml"

    sh "git push --follow-tags ${repo} ${branch}"

}