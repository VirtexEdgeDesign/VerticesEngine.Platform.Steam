using System;
using System.Collections.Generic;
using VerticesEngine.Platforms.Steam.Profile;
using VerticesEngine.Profile;
using VerticesEngine.Workshop.Events;
using VerticesEngine.Workshop;

namespace VerticesEngine.Platforms.Steam.Workshop
{
    /// <summary>
    /// The Steam Workshop Wrapper
    /// </summary>
    public class vxSteamWorkshopWrapper : vxIWorkshopWrapper
    {

        public bool IsPublishing
        {
            get { return _isPublishing; }
        }
        bool _isPublishing = false;

        public float PublishProgress
        {
            get { return _publishProgress; }
        }
        float _publishProgress = 0;


        public event EventHandler<vxWorkshopSeachReceievedEventArgs> SearchResultReceived;

        public event EventHandler<vxWorkshopItemPublishedEventArgs> ItemPublished;

        Facepunch.Steamworks.Workshop.Editor _item;

        /// <summary>
        /// The steam client which can be accessed by external 
        /// </summary>
        static internal Facepunch.Steamworks.Client SteamClient
        {
            get { return (vxPlayerProfileSteamWrapper.SteamClient); }
        }



        Facepunch.Steamworks.Workshop.Query query;
        public void Search(vxWorkshopSearchQuery searchCrteria, Action<vxWorkshopSearchResults> callback)
        {
            if (vxPlatform.Player.IsSignedIn)
            {
                // capture the search criteria here
                //vxWorkshop.OnSearch(searchCrteria);

                query = SteamClient.Workshop.CreateQuery();


                query.QueryType = Facepunch.Steamworks.Workshop.QueryType.SubscriptionItems;

                switch (searchCrteria.ItemCriteria)
                {
                    case vxWorkshopItemSearchCriteria.All:
                        query.UserQueryType = Facepunch.Steamworks.Workshop.UserQueryType.Published;
                        break;
                    //case vxWorkshopItemSearchCriteria.MyPublished:
                    //    query.UserQueryType = Facepunch.Steamworks.Workshop.UserQueryType.Published;
                    //    query.UserId = SteamClient.SteamId;
                    //    break;
                    //case vxWorkshopItemSearchCriteria.Subscribed:
                    //    query.UserQueryType = Facepunch.Steamworks.Workshop.UserQueryType.Subscribed;
                    //    query.UserId = SteamClient.SteamId;
                    //    break;
                    case vxWorkshopItemSearchCriteria.Favourited:
                        query.UserQueryType = Facepunch.Steamworks.Workshop.UserQueryType.Subscribed;
                        query.UserId = SteamClient.SteamId;
                        break;
                    //case vxWorkshopItemSearchCriteria.Followed:
                    //    query.UserQueryType = Facepunch.Steamworks.Workshop.UserQueryType.Followed;
                    //    query.UserId = SteamClient.SteamId;
                    //    break;
                }

                //SteamClient.Workshop.GetItem(0).
                foreach (var tag in searchCrteria.Tags)
                    query.RequireTags.Add(tag);

                //foreach (var tag in searchCrteria.TagsToExclude)
                //    query.ExcludeTags.Add(tag);


                //query.RequireTags.Add("Mod");


                if (searchCrteria.SearchText != "")
                    query.SearchText = searchCrteria.SearchText;
                //Workshop.UserQueryType.Subscribed
                //query.RequireTags.Add("Mod");
                query.OnResult = OnSearchResultReceived;
                query.Run();
            }
            else
            {
                //vxSceneManager.AddScreen(new vxMes)
            }
        }

        void OnSearchResultReceived(Facepunch.Steamworks.Workshop.Query obj)
        {
            List<vxIWorkshopItem> workshopItems = new List<vxIWorkshopItem>();

            if (obj.Items != null)
            {
                foreach (var item in obj.Items)
                {
                    if (item.Title != null && item.Title != "")
                        workshopItems.Add(new vxSteamWorkshopItem(item));
                }

                SteamClient.Workshop.OnItemInstalled += Workshop_OnItemInstalled;
                SearchResultReceived?.Invoke(this, new vxWorkshopSeachReceievedEventArgs(workshopItems));
            }
        }

        void Workshop_OnItemInstalled(ulong obj)
        {
            vxConsole.WriteNetworkLine("Installed: " + obj);
        }

        public void Publish(string title, string description, string imgPath, string uploadPath, string[] tags,
                           string idToUpdate = "", string changelog = "")
        {
            if (vxPlatform.Player.IsSignedIn)
            {
                Console.WriteLine("Uploading Folder: " + uploadPath);

                if (idToUpdate == "")
                {
                    vxConsole.WriteNetworkLine("Creating New Workshop Item...");
                    _item = SteamClient.Workshop.CreateItem(uint.Parse(vxEngine.Game.AppID), Facepunch.Steamworks.Workshop.ItemType.Community);
                }
                else
                {
                    vxConsole.WriteNetworkLine("Updating Workshop Item '" + idToUpdate + "'...");
                   if(ulong.TryParse(idToUpdate, out var steamItemId))
                        {
                        _item = SteamClient.Workshop.EditItem(steamItemId);
                        _item.WorkshopUploadAppId = uint.Parse(vxEngine.Game.AppID);
                    }

                }

                //Facepunch.Steamworks
                _item.Title = title;
                _item.Description = description;
                _item.Visibility = Facepunch.Steamworks.Workshop.Editor.VisibilityType.Public;

                _item.PreviewImage = imgPath;

                _item.Folder = uploadPath;

                _item.Tags.AddRange(tags);

                _item.ChangeNote = changelog;

                _item.Publish();

                vxConsole.WriteNetworkLine("Done");
            }
        }

        public void Update()
        {
            if (_item != null)
            {
                _isPublishing = _item.Publishing;
                if (_item.Publishing)
                {
                    if (Math.Abs(_item.Progress) < 0.01f)
                    {
                        _isPublishing = _item.Publishing;
                        Console.WriteLine("Publishing started, please wait.");
                    }
                    else
                    {
                        Console.WriteLine("Publishing: " + _item.Progress);
                        _publishProgress = (float)_item.Progress;
                    }
                }
                else
                {
                    _publishProgress = 0;
                    _isPublishing = false;

                    string info = (_item.Error == null) ? "Upload Successful!" : _item.Error;

                    ItemPublished?.Invoke(this, new vxWorkshopItemPublishedEventArgs(_item.Id.ToString(), (_item.Error == null), info));

                    vxConsole.WriteNetworkLine("Done publishing: ID: " + _item.Id + "\n" + _item.Error);

                    _item = null;
                }
            }
        }

        public string GetWorkshopItemURL(string id)
        {
            return $"https://steamcommunity.com/workshop/filedetails/?id={id}";
        }

        public void Download(vxIWorkshopItem id, Action<bool, string> callback)
        {
            // TODO
        }
    }
}