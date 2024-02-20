$(document).ready(function () {
    $('#articlesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut degil",
            "sInfo": "_TOTAL_ kayittan _START_ - _END_ arasindaki kayitlar gosteriliyor",
            "sInfoEmpty": "Kayit yok",
            "sInfoFiltered": "(_MAX_ kay�t i�erisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayit goster",
            "sLoadingRecords": "Y�kleniyor...",
            "sProcessing": "��leniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "E�le�en kay�t bulunamad�",
            "oPaginate": {
                "sFirst": "�lk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Onceki"
            },
            "oAria": {
                "sSortAscending": ": artan s�tun s�ralamas�n� aktifle�tir",
                "sSortDescending": ": azalan s�tun s�ralamas�n� aktifle�tir"
            },
            "select": {
                "rows": {
                    "_": "%d kay�t se�ildi",
                    "0": "",
                    "1": "1 kay�t se�ildi"
                }
            }
        }
    });
});