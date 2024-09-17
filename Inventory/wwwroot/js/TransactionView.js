    window.onload = () => {
        $("#filterId").on("keyup", function () {
            filterTable(0, $(this).val());
        });

        $("#filterDate").on("keyup", function () {
            filterTable(1, $(this).val());
        });

        $("#filterSeller").on("keyup", function () {
            filterTable(2, $(this).val());
        });

        $("#filterTransType").on("keyup", function () {
            filterTable(3, $(this).val());
        });

        $("#filterItem").on("keyup", function () {
            filterTable(4, $(this).val());
        });

        $("#filterPrice").on("keyup", function () {
            filterTable(5, $(this).val());
        });

        $("#filterQty").on("keyup", function () {
            filterTable(6, $(this).val());
        });

        $("#filterTotalPrice").on("keyup", function () {
            filterTable(7, $(this).val());
        });

        $("#filterRemainingQty").on("keyup", function () {
            filterTable(8, $(this).val());
        });

        function filterTable(column, value) {
            var table = $("#transactionTable");
            var rows = table.find("tbody tr");
            rows.each(function (index, row) {
                var td = $(row).find("td").eq(column);
                if (td.text().toLowerCase().indexOf(value.toLowerCase()) === -1) {
                    $(row).hide();
                } else {
                    $(row).show();
                }
            });
        }
    };