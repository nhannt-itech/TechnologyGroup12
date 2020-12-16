var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Account/GetAll"
        },
        "columns": [
            { "data": "username" },
            { "data": "role" },
            { "data": "employeeId" }
        ]
    });
}
