namespace TestCI.Domain.DrWallets
{
    public class DrWallet
    {
        public Guid Id_DRw { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? BillId { get; private set; }
        public StatusWallet Status { get; private set; }
        public DrWallet(
        Guid id_DRw,
        Guid clientId,
        StatusWallet status)
        {
            if (status == StatusWallet.Clsd)
                throw new InvalidOperationException(
                    "Cant be created CLsd");

            Id_DRw = id_DRw;
            ClientId = clientId;
            Status = status;
        }

        public void ChangeStatus(StatusWallet newStatus)
        {
            if (((Status == StatusWallet.Prcs) && (newStatus == StatusWallet.Actv)) ||
                ((Status == StatusWallet.Actv) && (newStatus == StatusWallet.Blck)) ||
                ((Status == StatusWallet.Blck) && (newStatus == StatusWallet.Actv)) ||
                ((Status == StatusWallet.Blck) && (newStatus == StatusWallet.Clsd)))
            { Status = newStatus; }
            else {
                throw new InvalidOperationException(
                    $"Cant be changed: {Status} -> {newStatus}");
            }
        }
        public void SetBillId(Guid IdBill)
        {
            if (BillId.HasValue)
            {
                throw new InvalidOperationException(
                    "Wallet account number is already set.");
            }

            BillId = IdBill;
        }
    }
}
