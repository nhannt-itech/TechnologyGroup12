var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var idBill = document.getElementById("billId").value;
    dataTable = $("#tblData").DataTable(
        {
            "bPaginate": true,
            "bFilter": false,
            "bInfo": false,
            "ajax": {
                "url": "/BillDetail/GetAllBillDetailOfBill/" + idBill
            },
            "columns": [
                { "data": "productName", "width": "15%" },
                { "data": "quantity", "width": "15%" },
                { "data": "totalPrice", "width": "15%" },
                { "data": "discount", "width": "15%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                <a href="/BillDetail/Update/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/BillDetail/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                    }, "width": "15%"
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
                            'Can not delete this, maybe it not exit or error from sever',
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