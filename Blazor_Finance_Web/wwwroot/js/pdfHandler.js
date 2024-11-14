async function downloadPdf() {
    // Obtenha os valores dos filtros
    const dataInicio = document.querySelector('input[name="dataInicio"]').value;
    const dataFim = document.querySelector('input[name="dataFim"]').value;
    let tipoConta = document.querySelector('select[name="tipoConta"]').value;
    let statusConta = document.querySelector('select[name="statusConta"]').value;

    // Substitua valores vazios por "Todos"
    tipoConta = tipoConta ? tipoConta : "Todos";
    statusConta = statusConta ? statusConta : "Todos";

    // Monta a URL com os parâmetros de filtro
    const url = `/api/report/contas-pdf?dataInicio=${dataInicio}&dataFim=${dataFim}&tipoConta=${tipoConta}&statusConta=${statusConta}`;

    const response = await fetch(url);
    if (response.ok) {
        const pdfUrl = await response.text();
        window.open(pdfUrl, '_blank');
    } else {
        // Trate o erro aqui, se necessário
        console.error('Erro ao gerar PDF:', response.statusText);
    }
}
