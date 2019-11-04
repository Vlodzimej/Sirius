export function GenerateErrorMessage(error: any): string {
    console.log(error);
    switch (error.status) {
        case 401: 
            return 'Выполните вход в систему';
        case 403:
            return 'Доступ ограничен';
        default:
            return 'Ошибка сервера: ('+error.status+') '+error.statusText
    }
}
