    $(document).ready(function () {
        $('#dataTables-example')
            .dataTable({
                responsive: true,
            });
    });


    $(function () {
        $('#datetimepicker1').datetimepicker({
            viewMode: 'years',
            format: 'MM/YYYY'

        });
    });
