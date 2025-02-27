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
        public const string OrderShipped = "S";
        public const string OrderDelivered = "D";
        public const string OrderCancelled = "C";
        public const string OrderCreated = "N";

        public const string OrderStatusCreated = "Created";
        public const string OrderStatusCompleted = "Completed";
        public const string OrderStatusCancelled = "Cancelled";

        public const string OrderItemStatusCreated = "Created";
        public const string OrderItemStatusShipped = "Shipped";
        public const string OrderItemStatusDelivered = "Delivered";
        public const string OrderItemStatusCancelled = "Cancelled";
        public const string OrderItemStatusInprogress = "In Progress";

        //Transaction Type
        public const string TransactionAdded = "Added";
        public const string TransactionUpdated = "Updated";
        public const string TransactionSold = "Sold";
        public const string TransactionReturn = "Return";
        public const string TransactionDeleted = "Deleted";

        //Connection String
        public const string ConnectionString = @"Server=(localdb)\\MSSQLLocalDB;Database=ims;Trusted_Connection=True;MultipleActiveResultSets=True";
    }
}
