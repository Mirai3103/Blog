export function pascalCaseToCamelCase(pascalCase: string){
    return pascalCase.charAt(0).toLowerCase() + pascalCase.slice(1);
}

interface ApiValidationError {
    [key: string]: string[];
}
export function parseValidateError(errors: ApiValidationError){
    const result: {[key: string]: string} = {};
    Object.keys(errors).forEach((key) => {
        result[pascalCaseToCamelCase(key)] = errors[key].join(', ');
    });
    return result;
}

type OnlyProperty<T extends object> = {
    [K in keyof T]: T[K] extends Function ?never :  K
}[keyof T];

