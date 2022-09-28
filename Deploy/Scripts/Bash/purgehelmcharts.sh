#! /bin/bash


if [ -z $1 ]
then
    echo "repository is required"
    exit 1
fi

if [ -z $2 ]
then
    echo "chart name is required"
    exit 1
fi

skip=$3
if [ -z $skip ]
then
    echo setting skip to 3
    skip=3
elif [ $skip -lt 3 ]
then
    echo resetting skip to 3
    skip=3
fi

top=$4
if [ -z $top ]
then
    echo setting top to 50
    top=50
elif [ $top -gt 50 ]
then
    echo resetting top to 50
    top=50
fi

lastIndex=`expr $skip + $top`

echo starting to purge the older versions of helm chart $2 of repository $1
echo $top versions after the latest $skip version will be purged
echo fetching older versions of the chart
propertyFilter=$(echo .[\"$2\"])
oldversions=$(az acr helm list -n $1 | jq $propertyFilter | jq 'sort_by(.created)' | jq 'reverse' | jq .[$skip:$lastIndex] | jq '.[] | .version')
for version in ${oldversions[*]}
do
    versionInt=$(echo $version | tr -d '"')
    echo purging the version $versionInt
    az acr helm delete -n $1 $2 --version $versionInt -y
done
echo purging completed successfully