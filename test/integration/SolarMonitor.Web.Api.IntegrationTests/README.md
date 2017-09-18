# TestHost-based Integration tests

<!-- TOC -->

- [TestHost-based Integration tests](#testhost-based-integration-tests)
    - [Known issues](#known-issues)
    - [To do](#to-do)

<!-- /TOC -->

These tests run across the full ASP.Net Pipeline using the .Net Core TestHost in-process test server.

Each test sets up an in-memory database (based on Sqlite), then send a request to the API server using the standard HttpClient. The API server runs in the same process as the tests using the TestHost framework. The requests are processed by the API server just as they would be if it ran in its own process hosted independently, however the TestHost allows the tests to run much faster and without additional setup steps.

## Known issues

When running the integration tests on Linux we run into the following exception:
`System.IO.IOException : The configured user limit (128) on the number of inotify instances has been reached.`

This is because by default the inotify instance limit is set to 128 for security reasons.

To work around this problem:

- open file: /etc/sysctl.conf
- insert: `fs.inotify.max_user_instances=10000`
- Reload configuration: `sudo sysctl -p`
- Verify the new limit with: `cat /proc/sys/fs/inotify/max_user_instances`

## To do

- Need to find out how to run tests in parallel on multiple cores. This should be just a configuration issue as the tests have no dependencies on each other and can be easily parallelized.
