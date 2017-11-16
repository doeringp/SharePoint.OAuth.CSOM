namespace SharePoint.OAuth.CSOM
{
    using Microsoft.SharePoint.Client;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Uri targetApplicationUri = new Uri("https://mytenantid.sharepoint.com/sites/mywebsitename");
            
            string targetRealm = TokenHelper.GetRealmFromTargetUrl(targetApplicationUri);

            var accessToken = TokenHelper.GetAppOnlyAccessToken
                (TokenHelper.SharePointPrincipal, targetApplicationUri.Authority, targetRealm).AccessToken;
                
            //we use the app-only access token to authenticate without the interaction of the user
            using (ClientContext context = TokenHelper.GetClientContextWithAccessToken(targetApplicationUri.ToString(), accessToken))
            {
                Web web = context.Web;

                context.Load(web);
                context.ExecuteQuery();
                Console.WriteLine(web.Title);

                List list = context.Web.Lists.GetByTitle("MyListName");
                CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
                ListItemCollection items = list.GetItems(query);
                context.Load(items);
                context.ExecuteQuery();
                foreach (ListItem item in items)
                {
                    Console.WriteLine(item["Title"]);
                }
            }
        }
    }
}
