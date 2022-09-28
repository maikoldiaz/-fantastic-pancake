FROM mcr.microsoft.com/oryx/python-3.7

# Web Site Home
ENV HOME_SITE "/home/site/wwwroot"
WORKDIR ${HOME_SITE}


# Flask uses port 5000
EXPOSE 5000
ENV PORT 5000
ENV SSH_PORT 2222


RUN apt-get update && apt-get install -y \
gcc \
g++ \
build-essential


RUN apt-get update && ACCEPT_EULA=Y apt-get upgrade -y

RUN pip install --upgrade pip
RUN pip install  pystan==2.19.1.1 #RUN pip install --upgrade pystan
RUN pip install  fbprophet==0.5
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# Install last version of bash (5.1)
RUN echo "deb http://deb.debian.org/debian bullseye main" | tee -a /etc/apt/sources.list
RUN apt update
RUN apt-get install bash -y
RUN bash --version 

COPY .  ${HOME_SITE}

ENTRYPOINT ["python"]


CMD ["/home/site/wwwroot/app.py"]