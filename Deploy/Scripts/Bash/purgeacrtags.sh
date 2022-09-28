#! /bin/bash


if [ -z $1 ]
then
    echo "registry is required"
    exit 1
fi

if [ -z $2 ]
then
    echo "repository name is required"
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

echo last index $lastIndex

echo starting to purge the older tags of repository $2 of registry $1
echo $top tags after the latest $skip tags will be purged
echo fetching old tags of the repository
oldtags=$(az acr repository show-tags -n $1 --repository $2 --orderby time_desc --query "[$skip:$lastIndex]" -o tsv)
for tag in ${oldtags[*]}
do
    image=$(echo $2:$tag)
    echo purging the tag $image
    az acr repository delete -n $1 --image $image -y
done
echo purging completed successfully