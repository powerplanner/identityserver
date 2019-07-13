# Identity server for Power Planner
The identity server for Power Planner accounts

Followed instructions from [here](https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4) for adding IdentityServer4 to the project

Also using [these instructions](https://mcguirev10.com/2018/01/02/identityserver4-without-entity-framework.html) for how to use a custom database store for user credentials



# Getting started

1. In the Azure Portal, open an existing Key Vault resource or create a new one
1. In the key vault, select **Certificates** and generate a new certificate, leave all the default options (make sure it's PKCS #12)
1. Now, create a new web app for the identity server
1. In the application settings, **add WEBSITE_LOAD_USER_PROFILE = 1**. Otherwise the certificate key will fail to load
1. Select the **Identity** option and enable System assigned identity which will allow setting up access to Azure Key Vault.
1. Switch back to the Key Vault, go to Access policies, and add a new one. Select the principal as the name of the web app, set the Certificate permissions to Get and List, and save.
1. Install Azure CLI and log in if you haven't yet
1. Deploy the site!