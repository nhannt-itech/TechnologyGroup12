var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $("#tblData").DataTable(
        {
            "bPaginate": true,
            "bFilter": false,
            "bInfo": false,
            "ajax": {
                "url": "/Discount/GetAll"
            },
            "columns": [
                { "data": "id"},
                { "data": "name" },
                {
                    "data": "discountValue",
                    "render": function (data) {
                        return (data * 100).toString() + '%';
                    }
                },
                {
                    "data": "startDate",
                    "render": function (data) {
                        return new Date(data).toLocaleString();
                    }
                },
                {
                    "data": "endDate",
                    "render": function (data) {
                        return new Date(data).toLocaleString();
                    }
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                <a href="/Discount/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Discount/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                    }
                }
            ]
        })
}

function SearchFor() {
    var columnName = document.getElementById("columnName").value;
    var searchFor = document.getElementById("searchFor").value;
    $("#tblData").dataTable().fnDestroy();
    $("#tblData").DataTable(
        {
            "bPaginate": true,
            "bFilter": false,
            "bInfo": false,
            "ajax": {
                "url": "/Discount/SearchFor/?columnName=" + columnName + "&searchFor=" + searchFor,
            },
            "columns": [
                { "data": "id" },
                { "data": "name" },
                {
                    "data": "discountValue",
                    "render": function (data) {
                        return (data * 100).toString() + '%';
                    }
                },
                {
                    "data": "startDate",
                    "render": function (data) {
                        return new Date(data).toLocaleString();
                    }
                },
                {
                    "data": "endDate",
                    "render": function (data) {
                        return new Date(data).toLocaleString();
                    }
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                <a href="/Discount/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Discount/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                    }
                }
            ]
        })
}

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#tblData').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            data.message,
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}