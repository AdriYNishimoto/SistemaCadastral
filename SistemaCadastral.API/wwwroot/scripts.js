function formatCpfCnpj(value) {
    if (!value) return '';
    value = value.replace(/\D/g, '');
    if (value.length <= 11) {
        return value
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
    } else {
        return value
            .replace(/(\d{2})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1/$2')
            .replace(/(\d{4})(\d{1,2})$/, '$1-$2');
    }
}

function formatCelular(value) {
    if (!value) return '';
    value = value.replace(/\D/g, '');
    return value
        .replace(/(\d{2})(\d)/, '($1) $2')
        .replace(/(\d{5})(\d)/, '$1-$2');
}

function calcularIdade(dataNasc) {
    if (!dataNasc) return '';
    const hoje = new Date();
    const nascimento = new Date(dataNasc);
    let idade = hoje.getFullYear() - nascimento.getFullYear();
    if (hoje.getMonth() < nascimento.getMonth() || 
        (hoje.getMonth() === nascimento.getMonth() && hoje.getDate() < nascimento.getDate())) {
        idade--;
    }
    return idade;
}