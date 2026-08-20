export function WalletRow(wallet) {
    const walletId = wallet.id_DRw || '—';
    const status = wallet.status || '—';
    const billDisplay = wallet.billId || 'Нет привязанного счета';
    return `
        <tr data-wallet-id="${walletId}">
            <td>${wallet.id_DRw}</td>
            <td class="status-cell" style="cursor: pointer; position: relative;">
                <span class="status-text">${status}</span>
            </td>
            <td>${billDisplay}</td>
        </tr>
    `;
}