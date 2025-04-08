import React, { useState, useEffect, useCallback } from 'react';
import { Select, Spin } from 'antd';
import debounce from 'lodash/debounce';
import { granjas } from '../../api/granjasAPI';
import { agroindustrias } from '../../api/agroindustriasAPI';

const SeletorBuscaDinamica = ({
  endpoint,
  placeholder,
  onChange,
  disabled,
  agroindustriaId
}) => {
  const [options, setOptions] = useState([]);
  const [fetching, setFetching] = useState(false);

  const getAPIMethod = useCallback((search) => {
    if (endpoint === '/granjas' && agroindustriaId) {
      return granjas.listarPorAgroindustria(agroindustriaId, search);
    }
    else if (endpoint === '/agroindustrias') {
      return agroindustrias.listarAgroindustrias(search);
    }
    else {
      throw new Error('Endpoint não suportado ou com erro');
    }
  }, [endpoint, agroindustriaId]);

/*   // Carrega valor inicial quando o componente monta (opcional)
  useEffect(() => {
    const loadInitialValue = async () => {
      if (!onChange) return; // Se o onChange não estiver definido, não faz sentido carregar o valor
      try {
        setFetching(true);
        const response = await getAPIMethod('');
        // Se o Form.Item está controlando o valor, o próprio Form deverá definir o valor atual.
        // Mas se você precisar carregar uma opção inicial que não esteja nas opções:
        // Suponha que você tenha acesso a um valor inicial (por exemplo, o valor atual do campo):
        // const valorAtual = value; // se você quiser usar o valor passado via Form, porém agora deixado de fora do componente.
        // Se valorAtual estiver presente:
        // const item = response.find(i => i.id === valorAtual);
        // if (item) { ... }
      } catch (error) {
        console.error('Erro ao carregar valor inicial:', error);
      } finally {
        setFetching(false);
      }
    };

    loadInitialValue();
  }, [getAPIMethod]); */

  const debouncedSearch = debounce(async (searchValue) => {
    try {
      setFetching(true);
      const response = await getAPIMethod(searchValue);

      const newOptions = response.map(item => ({
        value: item.id,
        label: endpoint === '/granjas'
          ? `${item.nomePropriedade} (${item.cnpj})`
          : `${item.nomeFantasia} (${item.cnpj})`
      }));

      setOptions(newOptions);
    } catch (error) {
      console.error('Erro na busca:', error);
      setOptions([]);
    } finally {
      setFetching(false);
    }
  }, 500);

  return (
    <Select
      showSearch
      placeholder={placeholder}
      onChange={onChange}
      filterOption={false}
      onSearch={debouncedSearch}
      notFoundContent={fetching ? <Spin size="small" /> : "Nenhum resultado"}
      options={options}
      optionFilterProp="label"
      style={{ width: '100%' }}
      disabled={disabled || (endpoint === '/granjas' && !agroindustriaId)}
      loading={fetching}
    />
  );
};

export default SeletorBuscaDinamica;