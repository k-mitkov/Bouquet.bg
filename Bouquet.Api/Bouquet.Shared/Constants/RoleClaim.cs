namespace Bouquet.Shared.Constants
{
    public static class RoleClaim
    {
        public const string AddBouquet = RoleClaimType.Permission + ".Bouquet.Add";
        public const string RemoveBouquet = RoleClaimType.Permission + ".Bouquet.Remove";
        public const string AddFlowerShop = RoleClaimType.Permission + ".Shop.Add";
        public const string AddWorker = RoleClaimType.Permission + ".Worker.Add";
        public const string ManageOrders = RoleClaimType.Permission + ".Orders.Manage";
        public const string ManageUsers = RoleClaimType.Permission + ".Users.Manage";
        public const string AddPermission = "Permision.Add";
        public const string RemovePermission = "Permision.Remove";
    }
}
