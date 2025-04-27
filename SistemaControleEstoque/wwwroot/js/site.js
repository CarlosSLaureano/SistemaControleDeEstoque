// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    getDatatable('#table-categorias');
    getDatatable('#table-produtos');
    getDatatable('#table-usuarios')
});

function getDatatable(id) {
    $(id).DataTable({
        dom: '<"top-bar d-flex justify-content-between"<"top-left"l><"top-right"f>>rt<"bottom-bar d-flex justify-content-between"<"bottom-left"i><"bottom-right"p>>',
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
            sSearch: "Pesquisar",
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
