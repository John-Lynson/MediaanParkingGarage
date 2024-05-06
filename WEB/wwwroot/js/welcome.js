<script>
    document.addEventListener('DOMContentLoaded', function() {
    const loginButton = document.querySelector('.btn-primary');
    loginButton.addEventListener('mouseover', function() {
        this.style.transform = 'scale(1.05)';
    });
    loginButton.addEventListener('mouseout', function() {
        this.style.transform = 'scale(1)';
    });
});
</script>
