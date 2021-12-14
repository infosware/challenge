import { PanelHogan } from '../panelHogan.js';
import { getUsers, requestGdpr, requestGdprDelete } from '../services/userService.js';
import { showLoadingOverlay, hideLoadingOverlay } from '../helpers.js';
import { Pagination } from './pagination.js';
import { DEFAULT_PAGE_NUMBER, DEFAULT_PAGE_SIZE } from '../constants.js';

class Users {
    constructor() {
        this.initSelectors();
        this.initComponents();
        this.initEvents();
        this.loadUsers(DEFAULT_PAGE_SIZE, DEFAULT_PAGE_NUMBER);
    }

    initSelectors() {
        this.selectors = {
            usersTemplate: 'users-hogan-template',
            usersWrapper: '.users_wrapper',
            userCheckboxAllCell: '.user-checkbox-all-cell',
            userCheckboxAll: '.user-checkbox-all',
            userCheckboxCell: '.user-checkbox-cell',
            userCheckbox: '.user-checkbox',
            btnAnonymize: '.btn-anonymize',
            btnDeanonymize: '.btn-deanonymize',
            usersTable: '.users_table',
            usersTableBody: '.entity-data-tbody',
        }
    }

    initComponents() {
        this.pagination = new Pagination({ callback: this.loadUsers, ctx: this });

        if (window[this.selectors.usersTemplate]) {
            this.hogan = new PanelHogan(this.selectors.usersTemplate);
        }
    }

    initEvents() {
        $(this.selectors.usersWrapper).on('click', (e) => this.onUsersClick(e));
    }

    onUsersClick(e) {
        const target = $(e.target);

        if (target.is($(this.selectors.userCheckboxAll)) || target.is($(this.selectors.userCheckboxAllCell))) {
            this.toggleUserCheckboxAll(target);
        } 
        else if (target.is($(this.selectors.userCheckboxCell))) {
            this.toggleUserCheckbox(target);
        }
        else if (target.is($(this.selectors.btnAnonymize))) {
            this.onGdprRequest(true);
        }
        else if (target.is($(this.selectors.btnDeanonymize))) {
            this.onGdprRequest(false);
        }

        this.toggleAnonButtons();
    }

    toggleUserCheckboxAll(target) {
        const cb = $(this.selectors.userCheckboxAll);
        const isChecked = cb.prop('checked');

        if (target.is($(this.selectors.userCheckboxAllCell))) {
            cb.prop('checked', !isChecked)
        }

        $(this.selectors.userCheckbox).each((index, checkbox) => $(checkbox).prop('checked', cb.prop('checked')));
    }

    toggleUserCheckbox(table_cell) {
        const cb = table_cell.children('input[type="checkbox"]');
        const isChecked = cb.prop('checked');

        cb.prop('checked', !isChecked);
    }

    toggleAnonButtons() {
        const isAnyUserChecked = $(this.selectors.usersTable).find('input:checked').length;

        $(this.selectors.btnAnonymize).prop('disabled', !isAnyUserChecked);
        $(this.selectors.btnDeanonymize).prop('disabled', !isAnyUserChecked);
    }

    onGdprRequest(anonymize) {
        const self = this;
        const emails = [];
        $(this.selectors.usersTableBody).find('input:checked').each((index, cb) => emails.push($(cb).val()));

        showLoadingOverlay($(self.selectors.usersWrapper));

        const gdprRequestFn = anonymize ? requestGdpr : requestGdprDelete;

        gdprRequestFn(emails).then((data) => {

            if (data && data.length) {
                toastr.error(data.join(`\n`), 'The following users are not found for updates:')
            }
            self.loadUsers(self.pageSize, self.pageNumber, self);
        })
        .catch(_ => {
            hideLoadingOverlay($(self.selectors.usersWrapper));
            toastr.error('An unexpected error occured')
        });;
    }

    loadUsers(pageSize, pageNumber, self) {
        if (!self) {
            self = this;
        }

        showLoadingOverlay($(self.selectors.usersWrapper));

        getUsers(pageSize, pageNumber).then((data) => {
            self.pageSize = pageSize;
            self.pageNumber = pageNumber;

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
            toastr.error('An unexpected error occured')
        });
    }
}

export { Users };

