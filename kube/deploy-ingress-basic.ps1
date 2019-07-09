##############################
# INGRESS CONTROLLER - Basic #
##############################

# Create a namespace
kubectl create namespace ingress-basic

# Install nginx-ingress
helm install stable/nginx-ingress `
    --namespace ingress-basic `
    --set controller.replicaCount=2 `
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux

# Check the service status
kubectl get service -l app=nginx-ingress --namespace ingress-basic

# Install the hello world chart
helm repo add azure-samples https://azure-samples.github.io/helm-charts/

# Install the app twice (two services) 
helm install azure-samples/aks-helloworld `
    --namespace ingress-basic `
    --set title="Hello AKS Ingress 1" `
    --set serviceName="aks-helloworld-1"

helm install azure-samples/aks-helloworld `
--namespace ingress-basic `
--set title="Hello AKS Ingress 2" `
--set serviceName="aks-helloworld-2"

kubectl apply -f hello-world-ingress.yaml