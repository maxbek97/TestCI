export function WalletRow(wallet) {
    return `
        <tr data-wallet-id="${wallet.id || wallet.mid || ''}">
            <td>${wallet.id || wallet.mid || '-'}</td>
            <td class="status-cell" style="cursor: pointer; position: relative;">
                <span class="status-text">${wallet.status || 'Active'}</span>
            </td>
            <td>${wallet.currency || wallet.details || '-'}</td>
        </tr>
    `;
}