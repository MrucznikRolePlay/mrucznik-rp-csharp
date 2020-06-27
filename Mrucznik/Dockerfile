#
# Build SAMP Sharp stage
#
#FROM ubuntu AS build-sampsharp
#
#WORKDIR /build
#
## install libraries
#RUN apt-get update && apt-get install -y \
#    premake4 \
#    g++ \
#    g++-multilib \
#    git \
#    make
#
## clone repository 
#RUN git clone https://github.com/ikkentim/SampSharp 
#
## build script
#RUN cd SampSharp && \
#    premake4 gmake && \
#    make config=release32
#



#
# Build gamemode stage
#
#FROM ubuntu AS build-gamemode
#
#WORKDIR /build


#
# SAMP server stage
#
FROM ubuntu

# Install libraries
RUN dpkg --add-architecture i386 && apt-get update && apt-get install -y \
    g++-multilib \ 
    wget \
    unzip \
    libicu66:i386

# Get SAMP server TODO: https://sa-mp.co.id/files/samp03DLsvr_R1.tar.gz
RUN mkdir /home/samp && \
    wget http://files.sa-mp.com/samp037svr_R2-1.tar.gz && \
    tar -xzf samp037svr_R2-1.tar.gz -C /home/samp --strip-components=1 && \ 
    rm samp037svr_R2-1.tar.gz
    
# Get SAMP Sharp resources
RUN wget https://github.com/ikkentim/SampSharp/releases/download/0.9.3/SampSharp-0.9.3.zip && \
    unzip SampSharp-0.9.3.zip && \
    cp -r SampSharp-0.9.3/* /home/samp && \
    rm -r SampSharp-0.9.3 && rm SampSharp-0.9.3.zip

# Get .NET Core
RUN wget https://deploy.timpotze.nl/packages/dotnet20200127.zip && \
    unzip dotnet20200127.zip && \
    cp -r dotnet20200127/runtime /home/samp/dotnet && \
    rm -r dotnet20200127 && rm dotnet20200127.zip

# Change config
RUN sed -i 's/changeme/funiol/' /home/samp/server.cfg && \ 
    sed -i 's/gamemode0 grandlarc 1/gamemode0 empty 1/' /home/samp/server.cfg && \
    sed -i 's/filterscripts base gl_actions gl_property gl_realtime/filterscripts/' /home/samp/server.cfg && \
    echo "plugins libSampSharp.so" >> /home/samp/server.cfg && \
    echo "coreclr dotnet" >> /home/samp/server.cfg && \
    echo "gamemode gamemode/Mrucznik.dll" >> /home/samp/server.cfg

#COPY --from=build-sampsharp /build/SampSharp/bin/* /home/samp/plugins/
#COPY --from=build-gamemode /build /home/samp/gamemode
COPY ./bin/Release/netcoreapp3.1/* /home/samp/gamemode/

EXPOSE 7777    

WORKDIR /home/samp
ENTRYPOINT ./samp03svr