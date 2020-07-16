# Create env dir
mkdir -p env

# Get SAMP server
wget https://sa-mp.co.id/files/samp03DLsvr_R1.tar.gz
tar -xzf samp03DLsvr_R1.tar.gz -C env --strip-components=1 
rm samp03DLsvr_R1.tar.gz

# Get SAMP Sharp resources
wget https://github.com/ikkentim/SampSharp/releases/download/0.9.3/SampSharp-0.9.3.zip
unzip SampSharp-0.9.3.zip
cp -r SampSharp-0.9.3/* env
rm -r SampSharp-0.9.3 && rm SampSharp-0.9.3.zip

# Get .NET Core
wget https://deploy.timpotze.nl/packages/dotnet20200127.zip
unzip dotnet20200127.zip
cp -r dotnet20200127/runtime env/dotnet
rm -r dotnet20200127 && rm dotnet20200127.zip
	
# Get crashdetect plugin
wget https://github.com/Zeex/samp-plugin-crashdetect/releases/download/v4.19.4/crashdetect-4.19.4-linux.tar.gz 
tar -xzf crashdetect-4.19.4-linux.tar.gz -C env/plugins --wildcards --no-anchored '*.so' --strip-components=1
rm crashdetect-4.19.4-linux.tar.gz
	
# Get streamer plugin
wget https://github.com/samp-incognito/samp-streamer-plugin/releases/download/v2.9.4/samp-streamer-plugin-2.9.4.zip
unzip -j samp-streamer-plugin-2.9.4.zip plugins/streamer.so -d env/plugins
rm samp-streamer-plugin-2.9.4.zip

# Change config
sed -i 's/changeme/funiol/' env/server.cfg 
sed -i 's/SA-MP 0.3 Server/!PL! Mrucznik Role Play 3.0 !PL!/' env/server.cfg 
sed -i 's/gamemode0 grandlarc 1/gamemode0 empty 1/' env/server.cfg
sed -i 's/filterscripts base gl_actions gl_property gl_realtime/filterscripts/' env/server.cfg
echo "plugins crashdetect.so libSampSharp.so streamer.so" >> env/server.cfg
echo "coreclr dotnet" >> env/server.cfg
echo "gamemode gamemode/Mrucznik.dll" >> env/server.cfg
