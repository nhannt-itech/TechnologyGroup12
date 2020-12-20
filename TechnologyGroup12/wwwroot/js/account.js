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
            {
                "data": "role",
                "render": function (data) {
                    if (data == 1) {
                        return 'ADMIN';
                    }
                    else {
                        return 'User';
                    }
                }
            },
            { "data": "employeeId" }
        ]
    });
}
