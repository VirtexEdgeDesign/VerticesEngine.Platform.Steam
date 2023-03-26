using Facepunch.Steamworks;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.Graphics;
using VerticesEngine.Plugins;
using VerticesEngine.Profile;
using VerticesEngine.UI.Controls;
using VerticesEngine.Utilities;


namespace VerticesEngine.Platforms.Steam.Profile
{
    /// <summary>
    /// Steam platform wrapper
    /// </summary>
    public sealed class vxPlayerProfileSteamWrapper : vxIPlayerProfile
    {
        public bool IsSignedIn
        {
            get { return _isSighnedIn; }
        }

        private bool _isSighnedIn = false;


        public string Name
        {
            get
            {
                return _steamUserName;
            }
        }
        private string _steamUserName = "";

        public string Id
        {
            get
            {
                return _steamUserID;
            }
        }
        string _steamUserID = "";


        public vxPlatformType PlatformType
        {
            get
            {
                return vxPlatformType.Steam;
            }
        }


        /// <summary>
        /// The steam client which can be accessed by external 
        /// </summary>
        internal static Facepunch.Steamworks.Client SteamClient
        {
            get { return _steamClient; }
        }
        static Facepunch.Steamworks.Client _steamClient;


        public Texture2D Avatar
        {
            get
            {
                return _steamUserAvatar;
            }
        }
        private Texture2D _steamUserAvatar;

        public string PreferredLanguage
        {
            get { return _preferredLanguage; }
        }

        public Dictionary<object, vxAchievement> Achievements 
        {
            get { return achievements; }
        }

        string _preferredLanguage = "english";



        /// <summary>
        /// Initialises a New Player Profile using Steam as the backend.
        /// </summary>
        internal vxPlayerProfileSteamWrapper()
        {

        }

        public void Initialise()
        {
            if (vxEngine.PlatformOS == vxPlatformOS.OSX)
            {
                Facepunch.Steamworks.Config.ForcePlatform(Facepunch.Steamworks.OperatingSystem.Osx, 
                                                          Facepunch.Steamworks.Architecture.x64);
            }
            
            _steamClient = new Facepunch.Steamworks.Client(uint.Parse(vxEngine.Game.AppID));            
        }

        public void SetStatus(string status)
        {
            _steamClient.User?.SetRichPresence("steam_display", status.ToSentanceCase());
            _steamClient.User?.SetRichPresence("status", status);
        }
        public void SetStatusKey(string key, string value)
        {
            _steamClient.User?.SetRichPresence(key, value.ToSentanceCase());
        }
        public void ClearStatus()
        {
            _steamClient.User?.ClearRichPresence();
        }

        public void SignIn()
        {
            // Make sure we started up okay
            _isSighnedIn = _steamClient.IsValid;

            if (_isSighnedIn)
            {
                _steamUserName = _steamClient.Username;
                _steamUserID = _steamClient.SteamId.ToString();

                //if (vxEngine.BuildType == vxBuildType.Debug)
                {
                    vxNotificationManager.Configs.IsOnBottom = false;
                    if (_steamUserAvatar != null)
                    {                        
                        vxNotificationManager.Show(vxLocalizer.GetText(vxLocKeys.Network_User_SignIn) + ": " + _steamClient.Username, _steamUserAvatar);

                    }
                    else
                    {
                        vxNotificationManager.Show(vxLocalizer.GetText(vxLocKeys.Network_User_SignIn) + ": " + _steamClient.Username, Microsoft.Xna.Framework.Color.Lime);
                    }

                    vxDebug.Log(new
                    {
                        //type = this.GetType() + ".SignIn()",
                        name = _steamClient.Username,
                        id = _steamClient.SteamId,
                        appid = _steamClient.AppId,
                        buildid = _steamClient.BuildId
                    }); ;
                }
            }
        }

        public void SignOut()
        {
            //throw new NotImplementedException();
        }

        public string GetAuthTicket()
        {
            var authTicket = _steamClient.Auth.GetAuthSessionTicket();
            byte[] ticketData = authTicket.Data;
            return System.BitConverter.ToString(ticketData, 0, ticketData.Length).Replace("-", string.Empty);
        }

        public string[] GetInstalledMods()
        {
            // for steam, there are two directories to search, the mod directory under 'My Games/...' and under the steam location

            List<string> mods = new List<string>();

            // the first local list under 'My Games/...'
            var regularList = vxPluginManager.GetAvailableModsInPath(vxIO.PathToMods);
            mods.AddRange(regularList);
            
            // now get the 
            if (IsSignedIn && _steamClient.InstallFolder != null)
            {
                DirectoryInfo dir = new DirectoryInfo(System.IO.Path.Combine(_steamClient.InstallFolder.FullName, "../../workshop/content", _steamClient.AppId.ToString()));

                var steamList = vxPluginManager.GetAvailableModsInPath(dir.FullName);
                mods.AddRange(steamList);
            }
            return mods.ToArray();
        }


        public void OpenURL(string url)
        {
            if(_steamClient != null && _isSighnedIn)
                _steamClient.Overlay.OpenUrl(url);
        }

        public void OpenStorePage(string url)
        {
            OpenURL(url);
        }

        //bool dl = false;
        public void Update()
        {
            try
            {
                if (_steamClient != null)
                    _steamClient.Update();
            }
            catch (Exception ex)
            {
                vxNotificationManager.Show(ex.Message, Microsoft.Xna.Framework.Color.Red);
            }
        }

        public Dictionary<object, vxAchievement> achievements = new Dictionary<object, vxAchievement>();

        public void AddAchievement(object key, vxAchievement achievement)
        {

        }

        public vxAchievement GetAchievement(object key)
        {
            return achievements[key];
        }

        public Dictionary<object, vxAchievement> GetAchievements()
        {
            return achievements;
        }


        public void IncrementAchievement(object key, int increment)
        {

        }

        public void ShareImage(string path, string extratxt = "")
        {

        }


        public void SubmitLeaderboardScore(string id, long score)
        {

        }

        public void UnlockAchievement(object key)
        {
            _steamClient.Achievements?.Trigger(key.ToString());
        }
        public void SetStat(string key)
        {
            _steamClient?.Stats?.Add(key);
        }

        public void SetStat(string key, int value)
        {
            _steamClient?.Stats?.Add(key, value);
        }

        public void ViewAchievments()
        {

        }

        public void ViewLeaderboard(string id)
        {

        }




#region Player Helper Methods

        /// <summary>
        ///     Get your steam avatar.
        ///     Important:
        ///     The returned Texture2D object is NOT loaded using a ContentManager.
        ///     So it's your responsibility to dispose it at the end by calling <see cref="Texture2D.Dispose()" />.
        /// </summary>
        /// <param name="device">The GraphicsDevice</param>
        /// <returns>Your Steam Avatar Image as a Texture2D object</returns>
        private Texture2D GetSteamUserAvatar()
        {
            if(_steamClient != null && _steamClient.SteamId != 0)
                _steamClient.Friends.GetAvatar(Facepunch.Steamworks.Friends.AvatarSize.Medium, _steamClient.SteamId, OnPlayerAvatarLoaded);
            
            return null;
        }


        public void GetPlayerIconFromPlatform(string id, Action<bool, Texture2D> callback)
        {
            if (_steamClient != null)
            {
                if (ulong.TryParse(id, out var steamid))
                {
                    _steamClient.Friends.GetAvatar(Friends.AvatarSize.Medium, steamid, (Image image) =>
                    {
                        if (image != null && image.IsLoaded)
                        {
                            var steamPlayerPhoto = new Texture2D(vxGraphics.GraphicsDevice, image.Width, image.Height);
                            steamPlayerPhoto.SetData<byte>(image.Data);
                            vxDebug.Log(new
                            {
                                id = _steamClient.SteamId,
                                width = image.Width,
                                height = image.Height
                            });

                      callback?.Invoke(true, steamPlayerPhoto);
                        }
                        else
                        {
                            vxConsole.WriteError($"Invalid Steam ID '{id}'");
                            callback?.Invoke(false, null);
                        }
                    });
                }
                else
                {
                    vxConsole.WriteError($"Invalid Steam ID '{id}'");
                    callback?.Invoke(false, null);
                }
            }
            else
            {
                vxConsole.WriteError($"Steam Client is not initialised for ID '{this.Name}'");
                callback?.Invoke(false, null);
            }
        }

        void OnPlayerAvatarLoaded(Facepunch.Steamworks.Image image)
        {
            if (image != null && image.IsLoaded)
            {                
                _steamUserAvatar = new Texture2D(vxGraphics.GraphicsDevice, image.Width, image.Height);
                Avatar.SetData<byte>(image.Data);
                vxDebug.Log(new
                {
                    id= _steamClient.SteamId,
                    width =image.Width,
                    height=image.Height
                });
                //vxConsole.WriteLine("Player Profile Image Loaded");
                //vxConsole.WriteLine("     Width:  "+image.Width);
                //vxConsole.WriteLine("     Height: " + image.Height);
            }
            else
            {
                vxConsole.WriteException(this, new Exception("Error Loading Avatar"));
            }
        }


        /// <summary>
        ///     Replaces characters not supported by your spritefont.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="input">The input string.</param>
        /// <param name="replaceString">The string to replace illegal characters with.</param>
        /// <returns></returns>
        public static string ReplaceUnsupportedChars(SpriteFont font, string input, string replaceString = "")
        {
            string result = "";
            if (input == null)
            {
                return null;
            }

            foreach (char c in input)
            {
                if (font.Characters.Contains(c) || c == '\r' || c == '\n')
                {
                    result += c;
                }
                else
                {
                    result += replaceString;
                }
            }
            return result;
        }



        public void Dispose()
        {

        }

        public void ViewAllLeaderboards()
        {

        }

        public void InitialisePlayerInfo()
        {
            vxConsole.WriteLine("Profile.InitialisePlayerInfo()");

            GetSteamUserAvatar();
        }


#endregion
    }
}