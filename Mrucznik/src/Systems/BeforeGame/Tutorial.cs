using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;

namespace Mrucznik.Systems.BeforeGame
{
    public class Tutorial
    {
        private readonly Player _player;
        private Timer _tutorialTimer;
        private int _tutorialTime;
        
        public Tutorial(Player player)
        {
            _player = player;
            _tutorialTime = 0;
			_player.Disconnected += TutorialOnDisconnect;
			_player.Text += TutorialOnText;
        }

        private void TutorialOnText(object sender, TextEventArgs e)
        {
            if(_tutorialTimer.IsRunning == true)
            {
				_player.SendClientMessage(Color.DarkRed,"Nie mo�esz pisa� podczas tutorialu!");
				e.SendToPlayers = false;
            }
        }

        private void TutorialOnDisconnect(object sender, EventArgs e)
        {
			if (_tutorialTimer.IsRunning == true) _tutorialTimer.Dispose();
		}

        public void RegisterMessage()
        {
	        _player.ClearChat();
            _player.SendClientMessage(Color.Yellow, "Witaj na Mrucznik Role Play serwer.");
            _player.SendClientMessage(Color.White, "Nie jest to serwer Full-RP ale obowi�zuj� tu podstawowe zasady RP.");
            _player.SendClientMessage(Color.White, "Je�li ich nie znasz przybli�e ci najwa�niejsz� zasade.");
            _player.SendClientMessage(Color.LightBlue, "Obowi�zuje absolutny zakaz DeathMatch`u(DM)");
            _player.SendClientMessage(Color.White, "Co to jest DM? To zabijanie graczy na serwerze bez konkretnego powodu.");
            _player.SendClientMessage(Color.White, "Chodzi o to, �e w prawdziwym �yciu, nie zabija�by� wszystkich dooko�a.");
            _player.SendClientMessage(Color.White, "Wi�c je�li chcesz kogo� zabi�, musisz mie� wa�ny pow�d.");
            _player.SendClientMessage(Color.White, "OK, znasz ju� najwa�niejsz� zasad�, reszt� poznasz p�niej.");
        }
        private void TutorialSend(int part)
        {
            if(part == 1)
            {
                _player.Position = new Vector3(849.62371826172, -989.92199707031, -5.0);
                _player.CameraPosition = new Vector3(849.62371826172, -989.92199707031, 53.211112976074);
                _player.SetCameraLookAt(new Vector3(907.40313720703, -913.14117431641, 77.788856506348));
                _player.ClearChat();
                _player.SendClientMessage(Color.Purple, "|____ Tutorial: Pocz�tek ____|");
                _player.SendClientMessage(Color.White, "Ooo... nowy na serwerze.... wi�c musisz o czym� wiedzie�.");
                _player.SendClientMessage(Color.White, "Jest to serwer Role Play(RP). Role Playing to odzwierciedlanie realnego �ycia w grze.");
                _player.SendClientMessage(Color.White, "Skoro ju� wiesz, co to jest Role Play musisz pozna� zasady panuj�ce na naszym serwerze.");
                _player.SendClientMessage(Color.White, "W tym celu przejdziesz teraz drobny samouczek tekstowy, kt�ry przygotuje Ci� do rozgrywki!");
            }
			else if (part == 14)
			{
				_player.Position = new Vector3(326.09194946289, -1521.3157958984, 20.0);
				_player.CameraPosition = new Vector3(398.16021728516, -1511.9237060547, 78.641815185547);// kamera
				_player.SetCameraLookAt(new Vector3(326.09194946289, -1521.3157958984, 42.154850006104));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zasady serwera - DM i Nick ____|");
			}
			else if (part == 16)
			{
				_player.SendClientMessage(Color.White, "Na serwerze obowi�zuje absolutny zakaz jakiegokolwiek DeathMatch`u(DM).");
				_player.SendClientMessage(Color.White, "Nie chcemy na serwerze os�b kt�re bezmy�lnie zabijaj� wszystko co si� rusza.");
				_player.SendClientMessage(Color.White, "Chodzi o to, �e w prawdziwym �yciu, nie zabija�by� wszystkich dooko�a.");
				_player.SendClientMessage(Color.White, "Wi�c je�li chcesz kogo� zabi�, musisz mie� naprawd� wa�ny pow�d.");
				_player.SendClientMessage(Color.White, "Na serwerze trzeba mie� nick typu Imie_Nazwisko (np. Jan_Kowalski)");
				_player.SendClientMessage(Color.White, "Je�li masz inny nick ni� Imie_Nazwisko to popro� admina o zmian� go.");
			}
			else if (part == 30)
			{
				_player.Position = new Vector3(1016.9872436523, -1372.0234375, -5.0);
				_player.CameraPosition = new Vector3(1053.3154296875, -1326.3295898438, 28.300031661987);// kamera
				_player.SetCameraLookAt(new Vector3(1016.9872436523, -1372.0234375, 15.836219787598));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zasady serwera - Bug Using i cheatowanie ____|");
			}
			else if (part == 32)
			{
				_player.SendClientMessage(Color.White, "Je�eli widzisz, �e kto� cheatuje, powiadom administrator�w przez komend� /report.");
				_player.SendClientMessage(Color.White, "Nie wolno u�ywa� Bug�w (np. znasz jakiego� buga kt�ry daje ci kase).");
				_player.SendClientMessage(Color.White, "Je�li masz czity, wy��cz je i id� na jaki� inny serwer. Tu nie mo�na czitowa�.");
				_player.SendClientMessage(Color.White, "Osoba korzystaj�ca z Cheat�w i Bug�w mo�e zosta� zbanowana lub ostrze�ona.");
			}
			else if (part == 52)
			{
				_player.Position = new Vector3(1352.2797851563, -1757.189453125, -5.0);
				_player.CameraPosition = new Vector3(1352.4576416016, -1725.1925048828, 23.291763305664);// kamera
				_player.SetCameraLookAt(new Vector3(1352.2797851563, -1757.189453125, 13.5078125));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zasady Serwera - OOC i IC ____|");
			}
			else if (part == 54)
			{
				_player.SendClientMessage(Color.White, "A wi�c musisz wiedzie� co to jest OOC i IC, oraz poprawnie to interpretowa�.");
				_player.SendClientMessage(Color.White, "Ta zasada jest bardzo wa�na, wi�c czytaj uwa�nie, oraz zapami�taj to.");
				_player.SendClientMessage(Color.White, "OOC to wszystko co NIE JEST zwi�zane z twoj� postaci�.(np. twoja szko�a).");
				_player.SendClientMessage(Color.White, "OOC to wszytskie rzeczy zwi�zane z tob� w realu, oraz z komendami, adminami itp.");
				_player.SendClientMessage(Color.White, "Rzeczy OOC piszemy w chatach: /b /o /i oraz /ro i /depo");
				_player.SendClientMessage(Color.White, "Poprawnie napisany tekst OOC: /b elo, jeste� adminem? /b jak tam w szkole? itp.");
			}
			else if (part == 74)
			{
				_player.Position = new Vector3(370.02825927734, -2083.5886230469, -10.0);
				_player.CameraPosition = new Vector3(340.61755371094, -2091.701171875, 22.800081253052);// kamera
				_player.SetCameraLookAt(new Vector3(370.02825927734, -2083.5886230469, 8.1386299133301));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zasady serwera - IC ____|");
			}
			else if (part == 76)
			{
				_player.SendClientMessage(Color.White, "Musisz te� wiedzie� co to jest IC, oraz poprawnie to interpretowa�.");
				_player.SendClientMessage(Color.White, "IC to tak jakby przeciwno�� OOC. To wszystko co JEST zwi�zane z twoj� postaci�.");
				_player.SendClientMessage(Color.White, "Rzeczy IC piszemy w chatach: /l /s /k /t /ad oraz w zwyk�ym chacie itp.");
				_player.SendClientMessage(Color.White, "Poprawnie napisany teskt IC: /l witam pana /k sta� policja, r�ce do g�ry! itp.");
			}
			else if (part == 96)
			{
				_player.Position = new Vector3(1172.8602294922, -1331.978515625, -5.0);
				_player.CameraPosition = new Vector3(1228.7977294922, -1345.1479492188, 21.532119750977);// kamera
				_player.SetCameraLookAt(new Vector3(1172.8602294922, -1331.978515625, 14.317019462585));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zasady serwera - MG i PG ____|");
			}
			else if (part == 98)
			{
				_player.SendClientMessage(Color.White, "MG (MetaGamming) - to wykorzystywanie informacji OOC do IC.");
				_player.SendClientMessage(Color.White, "Czyli widzisz nick nad g�ow� gracza i m�wisz do niego na chacie IC po imieniu");
				_player.SendClientMessage(Color.White, "Lub wtedy gdy kto� m�wi na /b jestem liderem LCN, a ty si� go pytasz o prac� w LCN");
				_player.SendClientMessage(Color.White, "PG - to zmuszanie kogo� do akcji RP, mimo i� ta osoba tego nie chce.");
				_player.SendClientMessage(Color.White, "Czyli np. kto� podchodzisz do kogo� i dajesz /me bije Johna tak �e umiera, to jest PG.");
			}
			else if (part == 112)
			{
				_player.Position = new Vector3(412.80743408203, -1312.4066162109, -5.0);
				_player.CameraPosition = new Vector3(402.2776184082, -1351.4703369141, 43.704566955566);// kamera
				_player.SetCameraLookAt(new Vector3(412.80743408203, -1312.4066162109, 39.677307128906));// patrz
				_player.SendClientMessage(Color.Purple, "|____ Tutorial: zako�czenie ____|");
			}
			else if (part == 114)
			{
				_player.SendClientMessage(Color.White, "Masz sie trzymac wymienionych zasad zrozumiano?.");
				_player.SendClientMessage(Color.White, "Poprostu pami�taj o nich i ciesz si� gr�, a jak nie... ");
				_player.SendClientMessage(Color.White, "Zapewne masz jeszcze sporo pyta� dotycz�cych gry. Spokojnie, znajdziesz na nie odpowied�!");
				_player.SendClientMessage(Color.White, "Mo�esz �mia�o pyta� administratora (/admins), poprzez zapytania (/zapytaj), b�d� te� [.]");
				_player.SendClientMessage(Color.White, "[.] poprzez chat dla nowych graczy /newbie. To ju� koniec samouczka. ");
				_player.SendClientMessage(Color.White, "Zasad, poradnik�w i pomocy jest znacznie wi�cej na naszym forum! Odwied� je: https://mrucznik-rp.pl");
			}
        }
        private void SetTutorialLogic()
        {
			_tutorialTime++;
			TutorialSend(_tutorialTime);
			_tutorialTimer = new Timer(1000, true);
            _tutorialTimer.Tick += _TutorialTimer_Tick;
        }

        private void _TutorialTimer_Tick(object sender, System.EventArgs e)
        {
            _tutorialTime++;
            if (_tutorialTime == 124)
                _tutorialTimer.Dispose();
			TutorialSend(_tutorialTime);
		}

        public void Start()
        {
            _player.ToggleControllable(false);
            _player.VirtualWorld = 0;
            _player.ToggleSpectating(true);
            SetTutorialLogic();
        }
    }
}