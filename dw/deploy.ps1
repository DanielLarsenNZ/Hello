$rg = 'hellodw-rg'
$location = 'westus2'
$storage = 'hellodwwus2'
$lake = 'hellodwlakewus2'
$tags = "project=hellodw"

# https://docs.microsoft.com/en-us/azure/storage/blobs/data-lake-storage-quickstart-create-account#create-an-account-using-azure-cli

# add the CLI extension
az extension add --name storage-preview

# Resource Group
& az group create -n $rg --location $location --tags $tags

# Create General Storage
& az storage account create `
    --name $storage `
    --resource-group $rg `
    --location $location `
    --sku Standard_LRS `
    --kind StorageV2 `
    --tags $tags

# Create Data Lake storage
& az storage account create `
    --name $lake `
    --resource-group $rg `
    --location $location `
    --sku Standard_LRS `
    --kind StorageV2 `
    --hierarchical-namespace true `
    --tags $tags
