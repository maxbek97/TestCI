export function ClientRow(props) {
    return `
        <tr>
            <td>${props.mid}</td>
            <td>${props.lastName} ${props.firstName}</td>
            <td>${props.email}</td>
            <td>${props.role}</td>
        </tr>
    `;
}