#! /bin/bash

if [ -z $1 ]
then
    echo "Certificate name is required"
    exit 1
fi

if [ -z $2 ]
then
    echo "Certificate path is required"
    exit 1
fi

if [ -z $3 ]
then
    echo "Certificate password is required"
    exit 1
fi

openssl pkcs12 -in $2 -nocerts -nodes -password pass:$3 2>/tmp/err
        if [ -s /tmp/err ]
          then
            echo "The certificate password is not configured correctly. please check the the cert key value pair in key vault for $1 certificate." >&2
          else
            echo "The certificate was validated successfully..."
        fi
