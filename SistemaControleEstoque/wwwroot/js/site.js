// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    getDatatable('#table-categorias');
    getDatatable('#table-produtos');
    getDatatable('#table-usuarios');
    getDatatable('#table-clientes');
    getDatatable('#table-logs');
});

function getDatatable(id) {
    if ($(id).length === 0) return;
    $(id).DataTable({
        dom: '<"top-bar d-flex justify-content-between align-items-center mb-3"<"top-left"l><"top-right"f>>rt<"bottom-bar d-flex justify-content-between align-items-center mt-3"<"bottom-left"i><"bottom-right"p>>',
        ordering: true,
        paging: true,
        searching: true,
        oLanguage: {
            sEmptyTable: "Nenhum registro encontrado na tabela",
            sInfo: "Mostrar _START_ até _END_ de _TOTAL_ registros",
            sInfoEmpty: "Mostrar 0 até 0 de 0 Registros",
            sInfoFiltered: "(Filtrar de _MAX_ total registros)",
            sLengthMenu: "Mostrar _MENU_ registros por página",
            sLoadingRecords: "Carregando...",
            sProcessing: "Processando...",
            sZeroRecords: "Nenhum registro encontrado",
            sSearch: "",
            sSearchPlaceholder: "Pesquisar...",
            oPaginate: {
                sNext: "Próximo",
                sPrevious: "Anterior",
            },
            oAria: {
                sSortAscending: ": Ordenar colunas de forma ascendente",
                sSortDescending: ": Ordenar colunas de forma descendente"
            }
        }
    });
}

$('.close-alert').click(function () {
    $('.alert').hide('hide');
});


function confirmDelete(url, itemName) {
    Swal.fire({
        title: 'Você tem certeza?',
        text: "Realmente deseja apagar " + (itemName || "este item") + "?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ef4444',
        cancelButtonColor: '#64748b',
        confirmButtonText: 'Sim, apagar!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = url;
        }
    });
}
