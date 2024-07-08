#!/bin/bash

ip_address=$(sudo ip -br a | awk '$1 == "enp0s3" {print $3}' | cut -d"/" -f1)

if [ -z "$ip_address" ]; then
    echo "IP-адрес не найден для enp0s3."
    exit 1
fi

echo "$ip_address" > /home/kirillok/script/ip_address.txt