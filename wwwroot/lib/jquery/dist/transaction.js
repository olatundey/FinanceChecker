$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#myTable').DataTable({
        "responsive": true,
        "ajax": {
            "url": '/Transaction/GetAllTransactions',
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "transactionID" },
            { "data": "accountID" },
            { "data": "institutionName" },
            { "data": "amount" },
            { "data": "category" },
            { "data": "description" },
            { "data": "date" },
            {
                "data": "transactionID",
                "render": function (data) {
                    return `<div class="btn-group" role="group">
            <a href="/Transaction/EditTransaction/upsert?transactionID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
            <a href="/Transaction/DeleteTransaction/upsert?transactionID=${data}" class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
        </div>`;
                }
            }
        ],
        "columnDefs": [
            {
                "targets": "_all",
                "responsivePriority": 1
            }
        ],
        "responsive": {
            "breakpoints": [
                { name: 'desktop', width: Infinity },
                { name: 'tablet', width: 1024 },
                { name: 'phone', width: 480 }
            ]
        }
    });
}
