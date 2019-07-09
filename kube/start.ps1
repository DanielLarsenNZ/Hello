. .\_vars.ps1

# & az login

& az account set --subscription $subscription

# Start the VMs
& az vm start --resource-group $nodesRG --name $node0 --no-wait
& az vm start --resource-group $nodesRG --name $node1 --no-wait
& az vm list --resource-group $nodesRG --show-details -o table

# Get Kube credentials and show details
& az aks get-credentials --resource-group $clusterRG --name $clusterName
& kubectl get nodes
& kubectl get services
