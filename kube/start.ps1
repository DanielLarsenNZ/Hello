. .\_vars.ps1

# & az login

& az account set --subscription 60d45c6e-19a3-49d3-9b40-c349d97761da
& az vm start --resource-group $nodesRG --name 'aks-agentpool-20008238-0' --no-wait
& az vm start --resource-group $nodesRG --name 'aks-agentpool-20008238-1' --no-wait
& az vm list --resource-group $nodesRG --show-details -o table

& kubectl get nodes
& kubectl get services
