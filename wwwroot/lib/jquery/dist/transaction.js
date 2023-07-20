$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#myTable').DataTable({
        "ajax": {
            "url": '/Account/GetAllTransactions', // Update the URL to match your controller action
            "type": "GET",
            "datatype": "json",
            "dataSrc": "" // The array of data is not wrapped in an object

        },
        "columns": [
            { "data": "transactionID", "width": "10%" },
            { "data": "accountID", "width": "10%" },
            { "data": "institutionName", "width": "15%" },
            { "data": "amount", "width": "10%" },
            { "data": "category", "width": "15%" },
            { "data": "description", "width": "20%" },
            { "data": "date", "width": "10%" },
            {
                "data": "transactionID",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
            <a href="/Account/EditTransaction/upsert?transactionID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
            <a href="/Account/DeleteTransaction/upsert?transactionID=${data}" class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
        </div>`;

                },
                "width": "10%"
            }
        ]
    });
}
