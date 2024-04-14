<script>
    function confirmOffline() {
            if (confirm("Are you sure you want to go offline?")) {
        window.location.href = '@Url.Action("Logout", "Account")';
            }
        }
</script>