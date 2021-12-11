import { PanelHogan } from '../panelHogan.js';

class Pagination {
    constructor(settings) {
        this.DEFAULT_PAGE_SIZE = 10;
        this.DEFAULT_PAGE_NUMBER = 1;

        this.settings = settings;

        this.initSelectors();
        this.initComponents();
        this.initEvents();
    }

    initSelectors() {
        this.selectors = {
            rowsPerPageBtn: '.paginator-rows',
            paginationWrapper: '.paginator-pages',
            paginationTemplate: 'pagination-hogan-template',
            goToFirstPageBtn: '.paginator-page-first',
            goToLastPageBtn: '.paginator-page-last',
            goToPreviousPageBtn: '.paginator-page-pre',
            goToNextPageBtn: '.paginator-page-next',
            goToPageBtn: '.paginator-page-number',
            goToPageAtMiddleBtn: '.paginator-page-number.many_pages_edges_middle',
            goToPageAtStartBtn: '.paginator-page-number.many_pages_middle_start',
            goToPageAtEndBtn: '.paginator-page-number.many_pages_middle_end',
        }
    }

    initComponents() {
        if (window[this.selectors.paginationTemplate]) {
            this.hogan = new PanelHogan(this.selectors.paginationTemplate);
        }
    }

    initEvents() {
        $(this.selectors.rowsPerPageBtn).on('click', (e) => this.onRowsPerPageChange(e));
        $(this.selectors.paginationWrapper).on('click', (e) => this.onPaginationChange(e));
    }

    onRowsPerPageChange(e) {
        $(this.selectors.rowsPerPageBtn).removeClass('active');
        $(e.currentTarget).addClass('active');

        const pageSize = +$(e.target).text().trim();
        this.settings.callback(pageSize, this.DEFAULT_PAGE_NUMBER, this.settings.ctx);
    }

    onPaginationChange(e) {
        const target = $(e.target).parent();

        switch (target.get(0)) {
            case $(this.selectors.goToFirstPageBtn).get(0):
                this.onGoToPageChange(1);
                break;
            case $(this.selectors.goToLastPageBtn).get(0):
                this.onGoToPageChange(this.numberOfPages);
                break;
            case $(this.selectors.goToPreviousPageBtn).get(0):
                this.onGoToPrevPageChange();
                break;
            case $(this.selectors.goToNextPageBtn).get(0):
                this.onGoToNextPageChange();
                break;
            case $(this.selectors.goToPageAtMiddleBtn).get(0):
                this.onGoToPageAtMiddleChange(target);
                break;
            case $(this.selectors.goToPageAtStartBtn).get(0):
                this.onGoToPageAtStartChange(target);
                break;
            case $(this.selectors.goToPageAtEndBtn).get(0):
                this.onGoToPageAtEndChange(target);
                break;
            default:
                if (target.get(0).className === $(this.selectors.goToPageBtn).get(0).className) {
                    this.onGoToPageChange(+target.text().trim());
                }
        }
    }

    onGoToPageChange(pageNumber) {
        this.settings.callback(this.currentPageSize, pageNumber, this.settings.ctx);
    }

    onGoToPrevPageChange() {
        const pageNumber = this.currentPageNumber > 1 ? this.currentPageNumber - 1 : 1;
        this.onGoToPageChange(pageNumber);
    }

    onGoToNextPageChange() {
        const pageNumber = this.currentPageNumber < this.numberOfPages ? this.currentPageNumber + 1 : this.numberOfPages;
        this.onGoToPageChange(pageNumber);
    }

    onGoToPageAtMiddleChange(target) {
        const pageNumber = +target.prev().text().trim();
        this.onGoToPageChange(pageNumber + 1);
    }

    onGoToPageAtStartChange(target) {
        const pageNumber = +target.next().text().trim();
        this.onGoToPageChange(pageNumber - 1);
    }

    onGoToPageAtEndChange(target) {
        const pageNumber = +target.prev().text().trim();
        this.onGoToPageChange(pageNumber + 1);
    }

    refreshPages(totalItems, rowsPerPage = this.DEFAULT_PAGE_SIZE, currentPage = this.DEFAULT_PAGE_NUMBER) {

        const pagesCount = Math.ceil(totalItems / rowsPerPage);
        const totalPages = [...Array(pagesCount || 0).keys()].map(i => i + 1); // 1 2 3 4 5

        this.numberOfPages = totalPages.length;
        this.currentPageNumber = currentPage;
        this.currentPageSize = rowsPerPage;

        const object = {};

        if (totalPages.length <= 10) {
            object.few_pages = totalPages.map((page) => ({
                pageNum: page,
                pageClass: page === currentPage ? 'active' : '',
            }));
        } else {
            if (currentPage <= 3 || currentPage > totalPages.length - 3) {
                object.many_pages_edge = { start: [], end: [] };
                object.many_pages_edge.start = totalPages.slice(0, 3).map((page) => ({
                    pageNum: page,
                    pageClass: page === currentPage ? 'active' : '',
                }));
                object.many_pages_edge.end = totalPages.slice(-3).map((page) => ({
                    pageNum: page,
                    pageClass: page === currentPage ? 'active' : '',
                }));
            } else if (currentPage > 3 && currentPage < totalPages.length - 3) {
                object.many_pages_middle = { middle: [] };
                object.many_pages_middle.middle = totalPages.slice(currentPage - 3, currentPage + 2).map((page) => ({
                    pageNum: page,
                    pageClass: page === currentPage ? 'active' : '',
                }));
            }
        }

        if (this.hogan) {
            const $pages = this.hogan.renderTemplate(object);
            $(this.selectors.paginationWrapper).empty().append($pages);
        }

        $(`${this.selectors.goToPageBtn}:contains('${currentPage}')`).addClass('active');
    }
}

export { Pagination };
