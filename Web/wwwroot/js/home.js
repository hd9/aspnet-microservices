// newsletter
var nlApp = new Vue({
    el: '#nlApp',
    data: {
        submitted: false,
        name: '',
        email: ''
    },
    methods: {
        submit: function () {
            if (this.name.length < 5 || this.email.length < 5) {
                alert("Names and emails should have at least 5 characters");
                return;
            }

            axios
                .post('/api/signup', { Name: this.name, Email: this.email })
                .then(response => {
                    this.submitted = true;
                })
                .catch(error => console.log(error));

            this.submitted = true;
        },
        resubmit: function () {
            this.submitted = false;
            this.name = '';
            this.email = '';
        }
    }
});
