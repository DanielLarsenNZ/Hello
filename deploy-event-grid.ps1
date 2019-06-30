# Enable Event Grid
#& az provider register --namespace Microsoft.EventGrid

<#
az eventgrid event-subscription create --name es2 \
--source-resource-id
"/subscriptions/{SubID}/resourceGroups/{RG}/providers/Microsoft.Storage/storageaccounts/s1"
\
--endpoint https://contoso.azurewebsites.net/api/f1?code=code \
--deadletter-endpoint /subscriptions/{SubID}/resourceGroups/TestRG/providers/Microsoft.S
torage/storageAccounts/s2/blobServices/default/containers/blobcontainer1 \
--max-delivery-attempts 10 --event-ttl 120
#>