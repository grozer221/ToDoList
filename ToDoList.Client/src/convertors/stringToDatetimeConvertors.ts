import moment from "moment";

export const stringToUSDate = (dateString: string): string => {
    const date = new Date(Date.parse(dateString));
    return moment(date).format('YYYY-MM-DD');
}

export const stringToUSTime = (dateString: string): string => {
    const date = new Date(Date.parse(dateString));
    return moment(date).format('hh:mm:ss');
}

export const stringToUSDatetime = (dateString: string): string => {
    if (!dateString)
        return '';
    return `${stringToUSDate(dateString)} ${stringToUSTime(dateString)}`;
}


export const camelCaseToString = (str: string) => str.replace(/[A-Z]/g, letter => ` ${letter.toLowerCase()}`);
