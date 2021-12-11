import { PanelHogan } from '../panelHogan.js';
import { getUsers } from '../services/userService.js';
import { showLoadingOverlay, hideLoadingOverlay } from '../helpers.js';
import { Pagination } from './pagination.js';

class Users {
    constructor() {
        this.initSelectors();
        this.initComponents();
        this.loadUsers();
    }

    initSelectors() {
        this.selectors = {
            usersTemplate: 'users-hogan-template',
            usersWrapper: '.users_wrapper',
        }
    }

    initComponents() {
        this.pagination = new Pagination({ callback: this.loadUsers, ctx: this });

        if (window[this.selectors.usersTemplate]) {
            this.hogan = new PanelHogan(this.selectors.usersTemplate);
        }
    }

    loadUsers(pageSize, pageNumber, self) {
        if (!self) {
            self = this;
        }

        showLoadingOverlay($(self.selectors.usersWrapper));

        getUsers(pageSize, pageNumber).then((data) => {

            const object = {
                users: [],
            };

            if (data && data.users) {
                object.users = data.users.map((user) => ({
                    id: user.id,
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                    createdAt: dayjs(user.createdAt).format('DD.MM.YYYY HH:mm'),
                }));

                if (self.hogan) {
                    const $users = self.hogan.renderTemplate(object);
                    $(self.selectors.usersWrapper).empty().append($users);
                }

                self.pagination.refreshPages(data.totalCount, pageSize, pageNumber);

            }

            hideLoadingOverlay($(self.selectors.usersWrapper));
        })
            .catch(_ => {
                hideLoadingOverlay($(self.selectors.usersWrapper));
            });
    }
}

export { Users };

