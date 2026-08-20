export function WalletRow(wallet) {
    const walletId = wallet.id_DRw || '—';
    const status = wallet.status || '—';
    const hasBill = Boolean(wallet.billId);
    const billDisplay = wallet.billId || 'Нет привязанного счета';
    return `
        <tr data-wallet-id="${walletId}">
            <td>${walletId}</td>
            <td class="status-cell" style="cursor: pointer; position: relative;">
                <span class="status-text">${status}</span>
            </td>
            <td class="bill-cell" style="cursor: pointer; position: relative;" data-has-bill="${hasBill}">
                <span class="bill-text">${billDisplay}</span>
            </td>
        </tr>
    `;
}