namespace InventoryManagement.Common
{
    public static class CommonStrings
    {
        //Roles
        public const string User = "User";
        public const string Admin = "Admin";
        public const string Supplier = "Supplier";

        //Order Status
        public const string OrderPending = "P";
        public const string OrderShipper = "S";
        public const string OrderDelivered = "D";
        public const string OrderCancelled = "C";
        public const string OrderCreated = "N";

        //Transaction Type
        public const string TransactionAdded = "Added";
        public const string TransactionUpdated = "Updated";
        public const string TransactionSold = "Sold";
        public const string TransactionDeleted = "Deleted";

        //Connection String
        public const string ConnectionString = @"Server=(localdb)\\MSSQLLocalDB;Database=ims;Trusted_Connection=True;MultipleActiveResultSets=True";
    }
}
