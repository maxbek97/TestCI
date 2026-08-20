export function ClientRow(client) {
    const fullName = `${client.lastName || ''} ${client.fisrtName || ''} ${client.middleName || ''}`.trim();
    return `
        <tr class="clickable-row" data-mid="${client.mid}" data-fullname="${fullName}" style="cursor: pointer;">
            <td>${client.mid || ''}</td>
            <td>${client.lastName || ''}</td>
            <td>${client.fisrtName || ''}</td>
            <td>${client.middleName || ''}</td>
            <td>${client.idDr || 'Нет кода на ЦР'}</td>
        </tr>
    `;
}