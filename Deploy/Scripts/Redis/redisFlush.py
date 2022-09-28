import argparse
import redis
import sys

parser = argparse.ArgumentParser() 
parser.add_argument("-n", "--Hostname", help = "Redis Host Name")
parser.add_argument("-p", "--Password", help = "Redis Password") 
args = parser.parse_args() 

if args.Hostname is None or args.Password is None :
    sys.exit("Please Pass Hostname and password")

r = redis.StrictRedis(host=args.Hostname,port=6380, db=0, password=args.Password, ssl=True)
r.flushall()