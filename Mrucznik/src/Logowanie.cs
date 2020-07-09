using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grpc.Core;
using Mruv;
using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using Timer = System.Timers.Timer;

namespace Mrucznik
{
	class Logowanie : BasePlayer
	{
		private Timer _realWorldTimeTimer;
		public bool IsLoggedIn { get; set; }
		public int LoginWarnings { get; set; }
		public void Logowanie()
		{
			_realWorldTimeTimer = new Timer(1000);
			_realWorldTimeTimer.Elapsed += (sender, args) => { SetTime(DateTime.Now.Hour, DateTime.Now.Minute); };
			_realWorldTimeTimer.AutoReset = true;
		}

		public override void OnConnected(EventArgs e)
		{
			base.OnConnected(e);
			IsLoggedIn = false;
			_realWorldTimeTimer.Start();
			SendClientMessage(Color.White, "SERVER: Witaj {0}", Name);

			if (!Regex.IsMatch(Name, "^[A-Z][a-z]+(_[A-Z][a-z]+([A-HJ-Z][a-z]+)?){1,2}$"))
			{
				SendClientMessage(
					"SERWER: Twój nick jest niepoprawny! Nick musi posiadać formę: Imię_Nazwisko!");
				Kick();
				return;
			}

			// Set time to real world time
			ToggleClock(true);

			Color = Color.LightGray;
			VirtualWorld = Id;
			ToggleControllable(false);

			var sounds = new[] { 1187, 171, 176, 1076, 1187, 157, 162, 169, 178, 180, 181, 147, 140 };
			PlaySound(sounds[new Random().Next(sounds.Length)]);

			Task.Delay(1000).ContinueWith(t =>
			{
				if (!IsConnected) return;

				Position = new Vector3(-2819.9297f, 1134.0607f, 26.0766f);
				Angle = 326.0f;
				CameraPosition = new Vector3(-2801.6691f, 1151.7545f, 31.5482f);
				SetCameraLookAt(new Vector3(-2819.05078f, 1141.4909f, 23.3147f));
				ApplyAnimation("ON_LOOKERS", "wave_loop", 3.5f, true, false, false, false, 0, false);
			});

			// Login/Registration
			var check = MruV.Accounts.IsAccountExists(new IsAccountExistsRequest() { Login = Name });
			var loginDialog = new InputDialog("Logowanie", $"Witaj {Name}. Twoje konto jest zarejestrowane\nZaloguj się wpisując w okienko poniżej hasło.\nJeżli nie znasz hasła do tego konta, wejdź pod innym nickiem.", true, "Zaloguj się", "Wyjdź");
			if (check.Exists)
			{
				loginDialog.Show(this);
				loginDialog.Response += (sender, args) =>
				{
					var req = new LogInRequest() { Login = Name, Password = args.InputText };
					try
					{
						LogInResponse response = MruV.Accounts.LogIn(req);
						if (response.Success)
						{
							IsLoggedIn = true;
							LoginWarnings = 0;
							SendClientMessage("Zostałeś poprawnie zalogowany do swojego konta.");
                            Player.Respawn();

						}
						else
						{
							SendClientMessage("Złe hasło!");
							LoginWarnings++;
							if (LoginWarnings == 3)
							{
								SendClientMessage("Wpisałeś 3 razy niepoprawne hasło!");
								Kick();
							}
							loginDialog.Show(this);
						}
					}
					catch (RpcException err)
					{
						SendClientMessage($"Nie udało się zalogować, błąd: {err.Status.Detail}");
					}
				};
			}
			else
			{
				var registerDialog = new InputDialog("Rejestracja konta", "Witaj. Aby zacząć grę na serwerze musisz się zarejestrować.\nAby to zrobić wpisz w okienko poniżej hasło które chcesz używać w swoim koncie.\nZapamiętaj je gdyż będziesz musiał go używać za każdym razem kiedy wejdziesz na serwer", true, "Zarejestruj się", "Wyjdź");
				registerDialog.Show(this);
				registerDialog.Response += (sender, args) =>
				{
					var req = new RegisterAccountRequest();
					req.Account = new Account()
					{
						Email = "mrucznix@gmail.com",
						Login = Name,
						Nick = Name
					};
					req.Password = args.InputText;

					try
					{
						var response = MruV.Accounts.RegisterAccount(req);
						if (response.Success)
						{
							SendClientMessage("Zarejestrowano!");
							loginDialog.Show(this);
						}
						else
						{
							SendClientMessage("Nie udało się zarejestrować konta.");
							registerDialog.Show(this);
						}
					}
					catch (RpcException err)
					{
						SendClientMessage($"Nie udało się zarejestrować, błąd: {err.Status.Detail}");
					}
				};
			}
		}

        public override void OnSpawned(SpawnEventArgs e)
        {
            if(IsLoggedIn == false)
            {
				Kick();
            }
        }

        public override void OnDisconnected(DisconnectEventArgs e)
		{
			base.OnDisconnected(e);
			IsLoggedIn = false;
			_realWorldTimeTimer.Stop();
		}

		public override void OnRequestClass(RequestClassEventArgs e)
		{
			base.OnRequestClass(e);

			ApplyAnimation("ON_LOOKERS", "wave_loop", 3.5f, true, false, false, false, 0, false);
		}
	}
}
