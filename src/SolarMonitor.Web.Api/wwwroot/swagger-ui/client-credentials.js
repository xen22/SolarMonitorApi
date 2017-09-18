(function () {
    clientId = "swagger";
    clientSecret = "secret";
    console.log("Set client ID and secret");
    // document.getElementsByClassName('.oauth-client-authentication-type').selectedIndex = 2;
    // document.getElementsByClassName('.oauth-client-id').value = clientId;
    // document.getElementsByClassName('.oauth-client-secret').value = clientSecret;

    console.log("type: ", document.getElementsByClassName('oauth-client-authentication-type').selectedIndex)
    console.log("drop down: ", document.getElementByName('client-authentication-type').value)
    console.log("client-id: ", document.getElementByName('client-id').value)
    console.log("client-secret: ", document.getElementByName('client-secret').value)

    document.getElementByName('client-authentication-type').selectedIndex = 2;
    document.getElementByName('client-id').value = clientId;
    document.getElementByName('client-secret').value = clientSecret;
    // $(function () {
    //     var basicAuthUi =
    //         '<div class="input">' +
    //         '<label text="Client Id" /><input placeholder="clientId" id="input_clientId" name="Client Id" type="text" size="25">' +
    //         '<label text="Client Secret" /><input placeholder="secret" id="input_secret" name="Client Secret" type="password" size="25">' +
    //         '</div>';

    //     $(basicAuthUi).insertBefore('div.info_title');
    //     $("#input_apiKey").hide();

    //     $('#input_clientId').change(addAuthorization);
    //     $('#input_secret').change(addAuthorization);
    // });

    // function addAuthorization() {
    //     var username = $('#input_clientId').val();
    //     var password = $('#input_secret').val();

    //     if (username && username.trim() !== "" && password && password.trim() !== "") {

    //         //What do I need to do here??
    //         //var basicAuth = new SwaggerClient.oauth2AUthorisation(username, password);
    //         //window.swaggerUi.api.clientAuthorizations.add("oauth2", basicAuth);

    //         console.log("Authorization added: ClientId = " +
    //             username + ", Secret = " + password);
    //     }
    // }
})();