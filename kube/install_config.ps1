# Install and config AKS. This script is not designed to be run. Cherry-pick the commands you need
# https://github.com/dotnet-architecture/eShopOnContainers/wiki/10.-Deploying-to-Kubernetes-(AKS-and-local)-using-Helm-Charts

. .\_vars.ps1

# Get creds for kubectl
& az aks get-credentials -g $clusterRG --n $clusterName

# Enable http_application_routing
& az aks enable-addons -g $clusterRG -n $clusterName --addons http_application_routing

# Show AKS config
& az aks show -g $clusterRG -n $clusterName

# Because the cluster is using RBAC, you need to grant needed rights to the Service Account kubernetes-dashboard with this kubectl command:
& kubectl create clusterrolebinding kubernetes-dashboard -n kube-system --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard

# Show the K8s dashboard
az aks browse --resource-group $clusterRG --name $clusterName

# https://docs.microsoft.com/en-us/azure/aks/kubernetes-helm
# Create the service account and role binding with the kubectl apply command
kubectl apply -f helm-rbac.yaml

# deploy a basic Tiller into an AKS cluster
helm init --service-account tiller